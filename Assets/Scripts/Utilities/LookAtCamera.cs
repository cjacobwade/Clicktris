using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : WadeBehaviour
{
	void LateUpdate()
	{
		if (Camera.main)
		{
			if (Camera.main.orthographic)
				transform.LookAt(transform.position + -Camera.main.transform.forward, Camera.main.transform.up);
			else
				transform.LookAt(Camera.main.transform, Camera.main.transform.up);
		}
	}
}
