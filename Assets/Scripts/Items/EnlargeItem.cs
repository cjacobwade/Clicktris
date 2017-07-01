using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlargeItem : CastItem
{
	protected override bool CanApply(RaycastHit hitInfo)
	{
		Dude dude = hitInfo.transform.GetComponent<Dude>();
		return dude.CanEnlarge();
	}

	protected override void ApplyEffect(RaycastHit hitInfo)
	{
		Dude dude = hitInfo.transform.GetComponent<Dude>();
		dude.Enlarge();
	}
}
