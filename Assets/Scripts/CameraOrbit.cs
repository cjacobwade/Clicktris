using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	void Awake()
	{
		UIManager.GetPanel<InventoryPanel>().gameObject.SetActive(true);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
			_prevMousePos = Input.mousePosition;

		if (unlockedLens)
		{
			if (Input.GetMouseButton(0))
				_inputVec = ((Vector2)Input.mousePosition - _prevMousePos);
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
