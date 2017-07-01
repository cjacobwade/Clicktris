using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LinqTools;

public class InventoryPanel : PanelBase
{
	[SerializeField]
	RectTransform _inventoryRect = null;
	public RectTransform GetInventoryRect()
	{ return _inventoryRect; }

	[SerializeField]
	RectTransform _leftRotateRect = null;
	public RectTransform GetLeftRotateRect()
	{ return _leftRotateRect; }

	[SerializeField]
	RectTransform _rightRotateRect = null;
	public RectTransform GetRightRotateRect()
	{ return _rightRotateRect; }

	BitSlotWidget[] _bitSlots = null;
	public BitSlotWidget[] GetBitSlots()
	{ return _bitSlots; }

	void Awake()
	{
		_bitSlots = _inventoryRect.GetComponentsInChildren<BitSlotWidget>();
	}
}
