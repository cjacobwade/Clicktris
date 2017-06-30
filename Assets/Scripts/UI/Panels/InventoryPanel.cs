using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : PanelBase
{
	[SerializeField]
	RectTransform _inventoryRect = null;
	public RectTransform GetInventoryRect()
	{ return _inventoryRect; }
}
