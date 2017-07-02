using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decor : WadeBehaviour
{
	[SerializeField]
	float _weight = 1f;
	public float GetWeight()
	{ return _weight; }

	[SerializeField]
	float _noiseThreshold = 0.45f;
	public float GetNoiseThreshold()
	{ return _noiseThreshold; }

	LookAtDirection _lookAtDirection = null;
	public LookAtDirection GetLookAtDirection()
	{ return _lookAtDirection; }

	[HideInInspector]
	public Vector3 initOffset = Vector3.zero;

	void Awake()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_lookAtDirection = GetComponent<LookAtDirection>();

		Planet.RegisterDecor(this);
	}

	void OnDestroy()
	{
		Planet.DeregisterDecor(this);
	}
}
