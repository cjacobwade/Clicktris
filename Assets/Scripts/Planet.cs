using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : Singleton<Planet>
{
	LookAtDirection[] _decorPrefabs = null;
	List<LookAtDirection> _decor = new List<LookAtDirection>();
	List<Vector3> _decorOffsets = new List<Vector3>();

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
	Vector2 _rotateSpeed = new Vector2(8f, 14f);

	[SerializeField]
	float _noiseScale = 5f;

	[SerializeField, Range(0f, 0.99f)]
	float _noiseSpawnThreshold = 0.3f;

	[SerializeField]
	float _decelSpeed = 5f;

	Vector2 _inputVec = Vector2.zero;
	Vector2 _prevMousePos = Vector2.zero;

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

			_decorOffsets.Add(spawnOffset);
			_decor.Add(decor);
		}

		_dudePrefabs = Resources.LoadAll<Dude>("Dudes");

		for(int i = 0; i < _numDudes; i++)
		{
			int dudeIndex = Random.Range(0, _dudePrefabs.Length);
			Dude dude = Instantiate<Dude>(_dudePrefabs[dudeIndex], transform);
			dude.transform.position = GetNearestSurfacePos(transform.position + Random.insideUnitSphere);

			_dudes.Add(dude);
		}
	}

	void FixedUpdate()
	{
		for(int i = 0; i < _decor.Count; i++)
		{
			_decor[i].transform.position = transform.position + transform.rotation * _decorOffsets[i];

			Vector3 toCamera = Camera.main.transform.position - _decor[i].transform.position;
			_decor[i].transform.position += Vector3.zero.SetZ(toCamera.z).normalized * _forwardOffset;

			Vector3 toDecor = (_decor[i].transform.position - transform.position).normalized;
			float edgeDot = Mathf.Abs(Vector3.Dot(toDecor, Vector3.forward));
			_decor[i].up = Vector3.Lerp(toDecor, Vector3.up, edgeDot);
		}

		if (Input.GetMouseButtonDown(0))
			_prevMousePos = Input.mousePosition;

		if (Input.GetMouseButton(0))
			_inputVec = ((Vector2)Input.mousePosition - _prevMousePos);
		else
			_inputVec -= Vector2.ClampMagnitude(_inputVec * _decelSpeed * Time.deltaTime, _inputVec.magnitude);

		Vector3 upVec = transform.InverseTransformDirection(Camera.main.transform.up);
		transform.Rotate(upVec, -_inputVec.x * _rotateSpeed.x * Time.deltaTime);

		Vector3 rightVec = transform.InverseTransformDirection(Camera.main.transform.right);
		transform.Rotate(rightVec, _inputVec.y * _rotateSpeed.y * Time.deltaTime);

		_prevMousePos = Input.mousePosition;	
	}

	public static Vector3 GetNormalAtPosition(Vector3 position)
	{ return (position - instance.transform.position).normalized; }

	public static Vector3 GetNearestSurfacePos(Vector3 position)
	{ return instance.transform.position + GetNormalAtPosition(position) * instance._scaleMesh.localScale.x / 2f; }
}