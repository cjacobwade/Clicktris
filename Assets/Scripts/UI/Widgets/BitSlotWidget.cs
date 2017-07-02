using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BitSlotWidget : WadeBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	Image _image = null;

	Color _initColor = Color.gray;

	[SerializeField]
	Color _highlightColor = Color.green;

	public bool used = false;

	ScaleTween _hoverTween = null;

	void Awake ()
	{
		_image = GetComponent<Image>();
		_initColor = _image.color;

		_hoverTween = GetComponent<ScaleTween>();
	}
	
	public void SetHighlight(bool highlight)
	{
		_image.color = highlight ? _highlightColor : _initColor;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_hoverTween.Play();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_hoverTween.Play(false);
	}
}
