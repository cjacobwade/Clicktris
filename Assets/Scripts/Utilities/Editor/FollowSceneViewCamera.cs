using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class FollowSceneViewCamera : Editor
{
	static bool _followSceneView = false;
	static Camera _followCamera = null;

	static FollowSceneViewCamera()
	{
		EditorApplication.update += Update;
	}

	[MenuItem("Sausage Soccer/Toggle Camera Follow Scene View &v")]
	public static void ToggleCameraFollowSceneView()
	{
		_followSceneView = !_followSceneView;
	}

	static void Update()
	{
		if (_followSceneView)
		{
			if (!_followCamera)
			{
				_followCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
				if (!_followCamera)
				{
					Debug.LogError("No CameraManager found to follow scene view");
					_followSceneView = false;
					return;
				}
			}

			var sceneCameras = SceneView.GetAllSceneCameras();
			if (sceneCameras.Length > 0)
			{
				_followCamera.transform.position = sceneCameras[0].transform.position;
				_followCamera.transform.rotation = sceneCameras[0].transform.rotation;
			}
			else
			{
				Debug.LogError("No scene view found to follow");
				_followSceneView = false;
				return;
			}
		}
	}
}
