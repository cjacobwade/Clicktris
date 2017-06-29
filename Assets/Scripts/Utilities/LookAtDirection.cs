using UnityEngine;
using System.Collections;

public class LookAtDirection : WadeBehaviour
{
	public Vector3 forward = Vector3.forward;
	public Vector3 up = Vector3.up;

	void LateUpdate()
	{
		transform.LookAt(transform.position + forward, up);
	}
}
