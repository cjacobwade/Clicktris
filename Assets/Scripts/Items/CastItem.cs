using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CastItem : WadeBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerExitHandler
{
	bool _down = false;
	bool _dragging = false;
	bool _toggled = false;

	RaycastHit _hitInfo = new RaycastHit();

	[SerializeField]
	LayerMask _rayLayer = 0;

	void Update()
	{
		if (_toggled && Input.GetMouseButtonDown(0))
		{
			Fire();

			_toggled = false;
			_down = false;
			_dragging = false;
		}
	}

	void Fire()
	{
		Vector3 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (Physics.Raycast(rayPos, Camera.main.transform.forward, out _hitInfo, Mathf.Infinity, _rayLayer, QueryTriggerInteraction.Ignore))
		{
			ApplyEffect(_hitInfo);
			Destroy(gameObject);
		}

		CameraOrbit.inputFreeLens.RemoveRequestsWithContext(this);
		CameraOrbit.unlockedLens.RemoveRequestsWithContext(this);
	}

	protected abstract void ApplyEffect(RaycastHit hitInfo);

	public void OnPointerDown(PointerEventData eventData)
	{
		_down = true;
		CameraOrbit.inputFreeLens.AddRequest(new LensHandle<bool>(this, false));
		CameraOrbit.unlockedLens.AddRequest(new LensHandle<bool>(this, false));
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (_dragging)
			Fire();

		_down = false;
		_dragging = false;
	}

	// Happens if press/release while on the button
	public void OnPointerClick(PointerEventData eventData)
	{
		CameraOrbit.inputFreeLens.AddRequest(new LensHandle<bool>(this, false));
		CameraOrbit.unlockedLens.AddRequest(new LensHandle<bool>(this, false));
		_toggled = true;
	}

	// Happens if mouse goes off of button, whether down or not
	public void OnPointerExit(PointerEventData eventData)
	{
		if (_down)
			_dragging = true;
	}
}