using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShadow : WadeBehaviour
{
	[SerializeField]
	SpriteRenderer _shadowPrefab = null;

	SpriteRenderer _shadow = null;
	public SpriteRenderer GetShadow()
	{ return _shadow; }

	[SerializeField]
	float _groundOffset = 0.1f;

	void Awake()
	{
		_shadow = Instantiate<SpriteRenderer>(_shadowPrefab, transform);
	}

	void Update()
	{
		SetPos(transform.position);
	}

	public void SetPos(Vector3 pos)
	{
		_shadow.transform.position = Planet.GetNearestSurfacePos(pos) + Planet.GetNormalAtPosition(pos) * _groundOffset;
		_shadow.transform.LookAt(_shadow.transform.position + Planet.GetNormalAtPosition(_shadow.transform.position));
	}
}
