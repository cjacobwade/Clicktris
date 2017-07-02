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
			{
				Transform blockPivot = blocks[i].GetRotatePivot();
				blockPivot.localScale = blockPivot.localScale.SetX(blockPivot.localScale.x * -1f);
			}

			Destroy(gameObject);
		}
	}
}
