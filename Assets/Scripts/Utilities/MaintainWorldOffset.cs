using UnityEngine;
using System.Collections;

public class MaintainWorldOffset : WadeBehaviour
{
	[SerializeField]
	Transform _offsetTarget = null;

	Vector3 _offset = Vector3.zero;

	void Awake()
	{
		_offset = _offsetTarget.position - transform.position;
	}

	void LateUpdate()
	{
		transform.position = _offsetTarget.position - _offset;
	}
}
