using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShadow : WadeBehaviour
{
	[SerializeField]
	SpriteRenderer _shadowPrefab = null;

	SpriteRenderer _shadow = null;

	[SerializeField]
	float _groundOffset = 0.1f;

	void Awake()
	{
		_shadow = Instantiate<SpriteRenderer>(_shadowPrefab, transform);
	}

	void Update()
	{
		_shadow.transform.position = Planet.GetNearestSurfacePos(transform.position) + Planet.GetNormalAtPosition(transform.position) * _groundOffset;
		_shadow.transform.LookAt(_shadow.transform.position + Planet.GetNormalAtPosition(_shadow.transform.position));
	}
}
