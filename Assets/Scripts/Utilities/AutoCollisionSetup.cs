using System.Collections;
using System.Collections.Generic;
using LinqTools;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class AutoCollisionSetup : WadeBehaviour
{
#if UNITY_EDITOR
	void OnEnable()
	{
		EditorApplication.hierarchyWindowChanged -= SetupCollision;
		EditorApplication.hierarchyWindowChanged += SetupCollision;
	}

	void OnDisable()
	{
		EditorApplication.hierarchyWindowChanged -= SetupCollision;
	}

	void SetupCollision()
	{
		if (!Application.isPlaying)
		{
			MeshRenderer[] meshChildren = transform.GetComponentsInChildren<MeshRenderer>();
			IEnumerable<GameObject> childMeshColliderGos = transform.GetComponentsInChildren<MeshCollider>().Select(mc => mc.gameObject);
			for (int i = 0; i < meshChildren.Length; i++)
			{
				if (!childMeshColliderGos.Contains(meshChildren[i].gameObject))
					meshChildren[i].AddComponent<MeshCollider>();
			}
		}
	}
#endif
}
