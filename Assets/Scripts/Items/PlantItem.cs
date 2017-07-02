using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantItem : CastPreviewItem
{
	[SerializeField]
	Decor _decorPrefab = null;

	protected override GameObject GetPreviewPrefab()
	{ return _decorPrefab.gameObject; }

	protected override void ApplyEffect(RaycastHit hitInfo)
	{
		Planet.SpawnDecor(_decorPrefab, hitInfo.point - hitInfo.normal * 0.15f);
	}
}
