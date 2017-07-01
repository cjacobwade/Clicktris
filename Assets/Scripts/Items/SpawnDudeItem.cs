using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinqTools;

public class SpawnDudeItem : CastItem
{
	[SerializeField]
	BlockType _dudeType = BlockType.Random;

	Dude _dudePrefab = null;

	void Awake()
	{
		while(_dudeType == BlockType.Random)
			_dudeType = ((BlockType[])System.Enum.GetValues(typeof(BlockType))).Random();

		_dudePrefab = Resources.Load<Dude>("Dudes/" + _dudeType.ToString());
	}

	protected override void ApplyEffect(RaycastHit hitInfo)
	{
		Vector3 spawnPos = Planet.GetNearestSurfacePos(hitInfo.point);
		Planet.SpawnDude(_dudePrefab, spawnPos);
	}
}
