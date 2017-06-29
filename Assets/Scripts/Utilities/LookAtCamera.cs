using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : WadeBehaviour
{
	[SerializeField]
	bool _rotateOnX = true;

	[SerializeField]
	bool _flipOnZ = true;

	float _initEulerX = 0f;

	void Start()
	{
		_initEulerX = transform.eulerAngles.x;
	}

	void LateUpdate()
	{
		if (Camera.main)
		{
			transform.LookAt(Camera.main.transform);

			if (!_rotateOnX)
				transform.eulerAngles = transform.eulerAngles.SetX(_initEulerX);

			if (_flipOnZ)
				transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
		}
	}
}
