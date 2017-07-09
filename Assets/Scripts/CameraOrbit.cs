using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraOrbit : Singleton<CameraOrbit>
{
	[SerializeField]
	Vector2 _rotateSpeed = new Vector2(8f, 14f);

	[SerializeField]
	float _decelSpeed = 5f;

#if UNITY_ANDROID
	[SerializeField]
	float _androidSpeedMod = 0.6f;
#endif

#if UNITY_IOS
	[SerializeField]
	float _iosSpeedMod = 0.6f;
#endif

	Vector2 _inputVec = Vector2.zero;
	Vector2 _downViewPos = Vector2.zero;
	Vector2 _prevMousePos = Vector2.zero;

	LensManager<bool> _unlockedLens = new LensManager<bool>(l => LensUtils.AllTrue(l));
	public static LensManager<bool> unlockedLens
	{ get { return instance._unlockedLens; } }

	LensManager<bool> _inputFreeLens = new LensManager<bool>(l => LensUtils.AllTrue(l));
	public static LensManager<bool> inputFreeLens
	{ get { return instance._inputFreeLens; } }

	int _clickStrength = 1;
	public static int GetClickStrength()
	{ return instance._clickStrength; }

	public static void AddClickStrength()
	{ instance._clickStrength++; }

	bool _down = false;
	bool _drag = false;

	[SerializeField]
	float _minDragThreshold = 0.17f;

	PointerEventData _eventData = null;
	List<RaycastResult> _uiHitResults = new List<RaycastResult>();

	void Awake()
	{
		UIManager.GetPanel<SwapPanel>().gameObject.SetActive(true);
		UIManager.GetPanel<BreedPanel>().gameObject.SetActive(true);

		_eventData = new PointerEventData(EventSystem.current);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_uiHitResults.Clear();
			_eventData.position = Input.mousePosition;
			EventSystem.current.RaycastAll(_eventData, _uiHitResults);

			if (_uiHitResults.Count == 0)
			{
				_down = true;
				_downViewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			}

			_prevMousePos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0))
		{
			_drag = false;
			_down = false;
		}

		if (unlockedLens)
		{
			if (Input.GetMouseButton(0) && _down)
			{
				if (!_drag)
				{
					Vector2 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
					Vector2 viewDelta = viewPos - _downViewPos;
					if (viewDelta.magnitude > _minDragThreshold)
						_drag = true;
				}

				if (_drag)
				{
					_drag = true;
					_inputVec = ((Vector2)Input.mousePosition - _prevMousePos);
				}
			}
			else
				_inputVec -= Vector2.ClampMagnitude(_inputVec * _decelSpeed * Time.deltaTime, _inputVec.magnitude);

			Vector2 speed = _rotateSpeed * Time.deltaTime;
#if UNITY_IOS && !UNITY_EDITOR
			speed *= _iosSpeedMod;
#elif UNITY_ANDROID && !UNITY_EDITOR
			speed *= _androidSpeedMod;
#endif

			transform.RotateAround(Planet.instance.transform.position, transform.up, _inputVec.x * speed.x);
			transform.RotateAround(Planet.instance.transform.position, transform.right, -_inputVec.y * speed.y);
		}
		else
			_inputVec = Vector2.zero;

		_prevMousePos = Input.mousePosition;
	}
}
