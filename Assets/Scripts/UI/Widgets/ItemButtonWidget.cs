using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonWidget : WadeBehaviour
{
	Image _background = null;

	[SerializeField]
	Image _foreground = null;

	[SerializeField]
	Image _icon = null;

	ScaleTween _slamTween = null;

	Color _bgDefaultColor = Color.white;
	Color _fgDefaultColor = Color.white;
	Color _iconDefaultColor = Color.white;
	Color _highlightColor = Color.green;

	bool _highlighted = false;

	[SerializeField]
	AnimationCurve _slotTweenScaleCurve = new AnimationCurve();

	[SerializeField]
	AnimationCurve _slotTweenPosCurve = new AnimationCurve();

	[SerializeField]
	float _flyToSlotTime = 0.8f;

	ItemSlotWidget _itemSlot = null;

	void Awake()
	{
		_background = GetComponent<Image>();
		_slamTween = GetComponent<ScaleTween>();

		_bgDefaultColor = _background.color;
		_fgDefaultColor = _foreground.color;
		_iconDefaultColor = _icon.color;
	}

	public void FlyToSlot(ItemSlotWidget slot)
	{
		_itemSlot = slot;
		StartCoroutine(SlotTweenRoutine());
	}

	protected void SetHighlighted(bool highlight)
	{
		if (highlight != _highlighted)
		{
			if (highlight)
			{
				_background.color *= _highlightColor;
				_foreground.color *= _highlightColor;
				_icon.color *= _highlightColor;
			}
			else
			{
				_background.color = _bgDefaultColor;
				_foreground.color = _fgDefaultColor;
				_icon.color = _iconDefaultColor;
			}

			_slamTween.Play();
		}
	}

	IEnumerator SlotTweenRoutine()
	{
		Vector3 startPosition = transform.position;
		Vector3 startScale = transform.localScale;

		float timer = 0f;
		while(timer < _flyToSlotTime)
		{
			timer += Time.deltaTime;

			float alpha = timer / _flyToSlotTime;
			transform.position = Vector3.Lerp(startPosition, _itemSlot.transform.position, _slotTweenPosCurve.Evaluate(alpha));
			transform.localScale = Vector3.Lerp(startScale, Vector3.one, _slotTweenScaleCurve.Evaluate(alpha));

			yield return null;
		}

		transform.SetParent(_itemSlot.transform);
	}
	
	void OnDestroy()
	{
		if (_itemSlot)
			_itemSlot.used = false;
	}
}
