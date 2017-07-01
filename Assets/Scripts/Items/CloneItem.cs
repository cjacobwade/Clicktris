using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneItem : CastItem
{
	protected override void ApplyEffect(RaycastHit hitInfo)
	{
		Dude dude = hitInfo.transform.GetComponent<Dude>();
		Dude newDude = Instantiate(dude, Planet.instance.transform);
		newDude.numEnlarges = dude.numEnlarges;
	}
}
