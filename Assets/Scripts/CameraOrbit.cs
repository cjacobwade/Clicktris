using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : WadeBehaviour
{
	[SerializeField]
	Vector2 _rotateSpeed = new Vector2(8f, 14f);

	[SerializeField]
	float _decelSpeed = 5f;

	Vector2 _inputVec = Vector2.zero;
	Vector2 _prevMousePos = Vector2.zero;

	void FixedUpdate()
	{
		if (Input.GetMouseButtonDown(0))
			_prevMousePos = Input.mousePosition;

		if (Input.GetMouseButton(0))
			_inputVec = ((Vector2)Input.mousePosition - _prevMousePos);
		else
			_inputVec -= Vector2.ClampMagnitude(_inputVec * _decelSpeed * Time.deltaTime, _inputVec.magnitude);

		transform.RotateAround(Planet.instance.transform.position, transform.up, _inputVec.x * _rotateSpeed.x * Time.deltaTime);
		transform.RotateAround(Planet.instance.transform.position, transform.right, -_inputVec.y * _rotateSpeed.y * Time.deltaTime);

		_prevMousePos = Input.mousePosition;
	}
}
