using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlipItem : WadeBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		Block[] blocks = FindObjectsOfType<Block>();
		if (blocks.Length > 0)
		{
			for (int i = 0; i < blocks.Length; i++)
				blocks[i].GetRotatePivot().localScale = blocks[i].GetRotatePivot().localScale.SetX(-1);

			Destroy(gameObject);
		}
	}
}
