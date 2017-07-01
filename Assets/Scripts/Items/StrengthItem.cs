using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StrengthItem : WadeBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		CameraOrbit.AddClickStrength();
		Destroy(gameObject);
	}
}
