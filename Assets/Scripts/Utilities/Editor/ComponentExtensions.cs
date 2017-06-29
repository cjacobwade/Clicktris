using UnityEngine;
using System.Collections;
using UnityEditor;

public class ComponentExtensions : WadeBehaviour
{
	[MenuItem("CONTEXT/Transform/Reset Children")]
	static void ResetChildren(MenuCommand command)
	{
		Transform transform = command.context as Transform;
		foreach(Transform t in transform)
		{
			t.position = Vector3.zero;
			t.rotation = Quaternion.identity;
			t.localScale = Vector3.one;
		}
	}
}
