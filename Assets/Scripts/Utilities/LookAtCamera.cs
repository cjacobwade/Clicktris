using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : WadeBehaviour
{
	void LateUpdate()
	{
		if (Camera.main)
			transform.LookAt(Camera.main.transform, Camera.main.transform.up);
	}
}
