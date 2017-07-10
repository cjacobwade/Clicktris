using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LookAtDirection))]
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

	[SerializeField, Range(-1f, 1f)]
	float _cullDot = 0f;
	public float GetCullDot()
	{ return _cullDot; }

	void Awake()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_lookAtDirection = GetComponent<LookAtDirection>();

		initOffset = transform.position - Planet.instance.transform.position;

		Planet.RegisterDecor(this);
	}

	void OnDestroy()
	{
		if(Planet.DoesExist())
			Planet.DeregisterDecor(this);
	}
}
