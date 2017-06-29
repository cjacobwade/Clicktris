using UnityEngine;
using System.Collections;

public class RandomRotate : WadeBehaviour
{
	[SerializeField]
	float _rotateSpeed = 5f;

	Vector3 _rotationVec = Vector3.zero;

	void Awake()
	{
		_rotationVec = Random.insideUnitSphere;
	}

	void FixedUpdate()
	{
		transform.rotation *= Quaternion.Euler(_rotationVec * _rotateSpeed * Time.deltaTime);
	}
}
