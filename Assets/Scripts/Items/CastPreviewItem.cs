using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CastPreviewItem : CastItem
{
	[SerializeField]
	Color _previewColor = Color.green;

	GameObject _preview = null;

	protected abstract GameObject GetPreviewPrefab();

	protected override void Update()
	{
		if (_toggled)
		{
			Vector3 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (Physics.Raycast(rayPos, Camera.main.transform.forward, out _hitInfo, Mathf.Infinity, _rayLayer, QueryTriggerInteraction.Ignore) && CanApply(_hitInfo))
			{
				_preview.gameObject.SetActive(true);
				_preview.transform.position = _hitInfo.point;
			}
			else
				_preview.gameObject.SetActive(false);

			if (Input.GetMouseButtonDown(0))
			{
				Fire();

				_toggled = false;
				_down = false;
				_dragging = false;

				Destroy(_preview.gameObject);
			}
		}

		if (_down && _dragging)
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * 5f;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);

		if (_toggled)
		{
			_preview = Instantiate(GetPreviewPrefab(), transform);
			_preview.GetComponentInChildren<SpriteRenderer>().color = _previewColor;
		}
	}
}