using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemWidget : WadeBehaviour
{
	[SerializeField]
	Text _name = null;

	[SerializeField]
	Text _description = null;

	[SerializeField]
	Image _image = null;

	// ShopItem _shopItem -> scriptableObject that implements effect? or just have a component on here?

	public void SetInfo(string name, string description, Sprite sprite)
	{
		_name.text = name;
		_description.text = description;
		_image.sprite = sprite;
	}
}