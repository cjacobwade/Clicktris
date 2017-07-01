using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : Singleton<Planet>
{
	LookAtDirection[] _decorPrefabs = null;
	List<LookAtDirection> _decor = new List<LookAtDirection>();
	List<SpriteRenderer> _decorSprite = new List<SpriteRenderer>();
	List<Vector3> _decorBasePoses = new List<Vector3>();

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

	[SerializeField, Range(0f, 0.99f)]
	float _noiseSpawnThreshold = 0.3f;

	void Awake()
	{
		_decorPrefabs = Resources.LoadAll<LookAtDirection>("Decor");

		for(int i = 0; i < _numDecor; i++)
		{
			int decorIndex = Random.Range(0, _decorPrefabs.Length);
			LookAtDirection decor = Instantiate<LookAtDirection>(_decorPrefabs[decorIndex], transform);
			decor.GetComponentInChildren<SpriteRenderer>().flipX = Random.value > 0.5f;

			Vector3 spawnOffset = Random.insideUnitSphere.normalized * 0.99f;
			while(Mathf.PerlinNoise((spawnOffset.x + spawnOffset.z) * _noiseScale, (spawnOffset.y + spawnOffset.z) * _noiseScale) < _noiseSpawnThreshold)
				spawnOffset = Random.insideUnitSphere.normalized * 0.99f;

			spawnOffset *= _scaleMesh.localScale.x / 2f;
			decor.transform.position = transform.position + spawnOffset;

			_decorBasePoses.Add(spawnOffset);
			_decorSprite.Add(decor.GetComponentInChildren<SpriteRenderer>());
			_decor.Add(decor);
		}

		_dudePrefabs = Resources.LoadAll<Dude>("Dudes");

		for(int i = 0; i < _numDudes; i++)
		{
			Dude dude = Instantiate<Dude>(_dudePrefabs[i], transform);
			dude.transform.position = GetNearestSurfacePos(transform.position + Random.insideUnitSphere);

			_dudes.Add(dude);
		}
	}

	void Update()
	{
		for(int i = 0; i < _decor.Count; i++)
		{
			Vector3 decorPos = transform.position + _decorBasePoses[i];
			decorPos += -Camera.main.transform.forward * _forwardOffset;

			Vector3 toDecor = (_decor[i].transform.position - transform.position).normalized;
			float edgeDot = Vector3.Dot(toDecor, Camera.main.transform.forward);

			_decorSprite[i].enabled = edgeDot < 0.6f;

			_decor[i].forward = -Camera.main.transform.forward;
			_decor[i].up = Vector3.Lerp(toDecor, Camera.main.transform.up, Mathf.Abs(edgeDot));

			_decor[i].transform.position = decorPos;
		}
	}

	public static Vector3 GetNormalAtPosition(Vector3 position)
	{ return (position - instance.transform.position).normalized; }

	public static Vector3 GetNearestSurfacePos(Vector3 position)
	{ return instance.transform.position + GetNormalAtPosition(position) * instance._scaleMesh.localScale.x / 2f; }
}