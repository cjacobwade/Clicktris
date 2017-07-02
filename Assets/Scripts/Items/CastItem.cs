using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CastItem : WadeBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler
{
	protected bool _down = false;
	protected bool _dragging = false;
	protected bool _toggled = false;

	protected RaycastHit _hitInfo = new RaycastHit();

	[SerializeField]
	protected LayerMask _rayLayer = 0;

	protected virtual bool CanApply(RaycastHit hitInfo)
	{ return true; }

	protected abstract void ApplyEffect(RaycastHit hitInfo);

	protected virtual void Update()
	{
		if (_toggled && Input.GetMouseButtonDown(0))
		{
			Fire();

			_toggled = false;
			_down = false;
			_dragging = false;
		}

		if (_down && _dragging)
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * 5f;
	}

	protected virtual void Fire()
	{
		Vector3 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (Physics.Raycast(rayPos, Camera.main.transform.forward, out _hitInfo, Mathf.Infinity, _rayLayer, QueryTriggerInteraction.Ignore) && CanApply(_hitInfo))
		{
			ApplyEffect(_hitInfo);
			Destroy(gameObject);
		}
		else
			transform.localPosition = Vector3.zero;

		CameraOrbit.inputFreeLens.RemoveRequestsWithContext(this);
		CameraOrbit.unlockedLens.RemoveRequestsWithContext(this);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_down = true;
		CameraOrbit.inputFreeLens.AddRequest(new LensHandle<bool>(this, false));
		CameraOrbit.unlockedLens.AddRequest(new LensHandle<bool>(this, false));
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (_dragging)
			Fire();
		else
		{
			CameraOrbit.inputFreeLens.AddRequest(new LensHandle<bool>(this, false));
			CameraOrbit.unlockedLens.AddRequest(new LensHandle<bool>(this, false));
			_toggled = true;
		}

		_down = false;
		_dragging = false;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (_down)
			_dragging = true;
	}
}