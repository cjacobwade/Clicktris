using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BitSlotWidget : WadeBehaviour
{
	Image _image = null;

	Color _initColor = Color.gray;

	[SerializeField]
	Color _highlightColor = Color.green;

	public bool used = false;

	void Awake ()
	{
		_image = GetComponent<Image>();
		_initColor = _image.color;
	}
	
	public void SetHighlight(bool highlight)
	{
		_image.color = highlight ? _highlightColor : _initColor;
	}
}
