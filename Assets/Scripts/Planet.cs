using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinqTools;

public class Planet : Singleton<Planet>
{
	List<Decor> _decorPrefabs = new List<Decor>();
	List<Decor> _decor = new List<Decor>();

	public static void RegisterDecor(Decor decor)
	{ instance._decor.Add(decor); }

	public static void DeregisterDecor(Decor decor)
	{ instance._decor.Remove(decor); }


	Dude[] _dudePrefabs = null;
	List<Dude> _dudes = new List<Dude>();

	[SerializeField]
	Transform _scaleMesh = null;

	[SerializeField, Range(0, 800)]
	int _numDecor = 200;

	[SerializeField]
	int _numDudes = 25;

	[SerializeField]
	float _forwardOffset = 0.1f;

	[SerializeField]
	float _noiseScale = 5f;

	void Awake()
	{
		_decorPrefabs = Resources.LoadAll<Decor>("Decor").ToList();

		for(int i = 0; i < _numDecor; i++)
		{
			_decorPrefabs.Sort((x, y) => (y.GetWeight() * Random.value).CompareTo(x.GetWeight() * Random.value));
			SpawnDecor(_decorPrefabs[0]);
		}

		_dudePrefabs = Resources.LoadAll<Dude>("Dudes");

		for(int i = 0; i < _numDudes; i++)
			SpawnDude(_dudePrefabs[i]);
	}

	public static void SpawnDecor(Decor decorPrefab, Vector3? spawnPos = null)
	{
		Decor decor = Instantiate<Decor>(decorPrefab, instance.transform);
		decor.spriteRenderer.flipX = Random.value > 0.5f;

		if (!spawnPos.HasValue)
		{
			Vector3 spawnOffset = Random.insideUnitSphere.normalized * 0.99f;
			float noise = Mathf.PerlinNoise(
				(spawnOffset.x + spawnOffset.z) * instance._noiseScale,
				(spawnOffset.y + spawnOffset.z) * instance._noiseScale);

			while (noise < decorPrefab.GetNoiseThreshold())
			{
				spawnOffset = Random.insideUnitSphere.normalized * 0.99f;
				noise = Mathf.PerlinNoise(
					(spawnOffset.x + spawnOffset.z) * instance._noiseScale,
					(spawnOffset.y + spawnOffset.z) * instance._noiseScale);
			}

			spawnOffset *= instance._scaleMesh.localScale.x / 2f;
			spawnPos = instance.transform.position + spawnOffset;
		}

		decor.transform.position = spawnPos.Value;
		decor.initOffset = spawnPos.Value - instance.transform.position;
	}
	
	public static Dude SpawnDude(Dude dudePrefab, Vector3? spawnPos = null)
	{
		Dude dude = Instantiate<Dude>(dudePrefab, instance.transform);

		if(!spawnPos.HasValue)
			spawnPos = GetNearestSurfacePos(instance.transform.position + Random.insideUnitSphere);

		dude.transform.position = spawnPos.Value;

		instance._dudes.Add(dude);

		return dude;
	}

	void Update()
	{
		Vector3 toCamera = (UIManager.GetPanel<BreedPanel>().GetViewRect().position - transform.position).normalized;
		for (int i = 0; i < _decor.Count; i++)
		{
			Vector3 decorPos = transform.position + _decor[i].initOffset;
			decorPos += -Camera.main.transform.forward * _forwardOffset;

			Vector3 toDecor = (_decor[i].transform.position - transform.position).normalized;
			float edgeDot = Vector3.Dot(toDecor, toCamera);

			_decor[i].spriteRenderer.enabled = edgeDot > _decor[i].GetCullDot();

			_decor[i].GetLookAtDirection().forward = -Camera.main.transform.forward;
			_decor[i].GetLookAtDirection().up = Vector3.Lerp(toDecor, Camera.main.transform.up, Mathf.Abs(edgeDot));

			_decor[i].transform.position = decorPos;
		}
	}

	public static Vector3 GetNormalAtPosition(Vector3 position)
	{ return (position - instance.transform.position).normalized; }

	public static Vector3 GetNearestSurfacePos(Vector3 position)
	{ return instance.transform.position + GetNormalAtPosition(position) * instance._scaleMesh.localScale.x / 2f; }
}