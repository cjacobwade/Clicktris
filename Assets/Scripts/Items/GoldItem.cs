using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoldItem : WadeBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		Block[] blocks = FindObjectsOfType<Block>();
		if (blocks.Length > 0)
		{
			for (int i = 0; i < blocks.Length; i++)
			{
				if (blocks[i].GetBitSprites()[0].enabled)
					blocks[i].SetGold(true);
			}

			Destroy(gameObject);
		}
	}
}
