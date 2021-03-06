﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LinqTools;

public class BreedPanel : PanelBase
{
	[SerializeField]
	RectTransform _breedRect = null;
	public RectTransform GetBreedRect()
	{ return _breedRect; }

	[SerializeField]
	RectTransform _viewRect = null;
	public RectTransform GetViewRect()
	{ return _viewRect; }

	[SerializeField]
	RectTransform _leftRotateRect = null;
	public RectTransform GetLeftRotateRect()
	{ return _leftRotateRect; }

	[SerializeField]
	RectTransform _rightRotateRect = null;
	public RectTransform GetRightRotateRect()
	{ return _rightRotateRect; }

	List<BitSlotWidget> _bitSlots = new List<BitSlotWidget>();
	public List<BitSlotWidget> GetBitSlots()
	{ return _bitSlots; }

	List<ItemSlotWidget> _itemSlots = new List<ItemSlotWidget>();
	ItemButtonWidget[] _itemButtonPrefabs = null;

	[SerializeField]
	Button _combineButton = null;

	List<BlockType> _addedBlockTypes = new List<BlockType>();

	[SerializeField]
	FloatRange _slotOffsetRange = new FloatRange(0.25f, 0.3f);

	void Awake()
	{
		_bitSlots = _breedRect.GetComponentsInChildren<BitSlotWidget>().ToList();

		_itemButtonPrefabs = Resources.LoadAll<ItemButtonWidget>("Items");
		_itemSlots = GetComponentsInChildren<ItemSlotWidget>().ToList();

		_leftRotateRect.gameObject.SetActive(false);
		_rightRotateRect.gameObject.SetActive(false);

		_combineButton.gameObject.SetActive(false);
	}

	public void AssignBlock(Block block)
	{
		foreach (var kvp in block.GetSpriteToSlotMap())
		{
			BitSlotWidget slot = kvp.Value;
			slot.used = true;

			Transform spriteParent = new GameObject(gameObject.name + "SpriteRoot").transform;
			spriteParent.localScale = block.transform.localScale;
			spriteParent.SetParent(slot.transform);

			float slotAlpha = _bitSlots.IndexOf(slot) / (float)_bitSlots.Count;
			spriteParent.position = slot.transform.position + -Camera.main.transform.forward * _slotOffsetRange.Lerp(slotAlpha);

			SpriteRenderer sprite = kvp.Key;
			sprite.transform.SetParent(spriteParent);
			sprite.transform.localPosition = Vector3.zero;
			sprite.GetComponent<Collider>().enabled = false;
		}

		string soundName = "SFX_BlockPlacement" + (Mathf.Clamp(_addedBlockTypes.Count, 0, 3) + 1);
		GameSound placeSound = (GameSound)System.Enum.Parse(typeof(GameSound), soundName);
		GameSounds.PostEvent2D(placeSound);

		_addedBlockTypes.Add(block.GetBlockType());

		if(_addedBlockTypes.Count > 1)
			_combineButton.gameObject.SetActive(true);
	}

	public void Combine()
	{
		_combineButton.gameObject.SetActive(false);

		GameSounds.PostEvent2D(GameSound.SFX_BlockCombine);

		// TODO: Calculate what item we need to give here

		ItemButtonWidget itemButton = Instantiate<ItemButtonWidget>(_itemButtonPrefabs.Random(), _breedRect);
		itemButton.transform.ResetLocals();

		for(int i = 0; i < _itemSlots.Count; i++)
		{
			if(!_itemSlots[i].used)
			{
				_itemSlots[i].used = true;
				itemButton.FlyToSlot(_itemSlots[i]);
				break;
			}
		}

		_addedBlockTypes.Clear();

		for (int i = 0; i < _bitSlots.Count; i++)
		{
			_bitSlots[i].used = false;

			foreach (Transform child in _bitSlots[i].transform)
				Destroy(child.gameObject);
		}
	}
}
