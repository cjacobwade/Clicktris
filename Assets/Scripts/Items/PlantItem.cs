using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantItem : CastItem
{
	[SerializeField]
	Decor _decor = null;

	protected override void ApplyEffect(RaycastHit hitInfo)
	{
		Planet.instance.SpawnDecor(_decor, hitInfo.point - hitInfo.normal * 0.15f);
	}
}
