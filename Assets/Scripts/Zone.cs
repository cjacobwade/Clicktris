using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Zone : WadeBehaviour
{
	LineRenderer _line = null;

	[SerializeField]
	int _numIterations = 100;

	[SerializeField]
	float _groundOffset = 0.4f;

	[SerializeField]
	float _zoneRadius = 2f;

	void Awake ()
	{
		_line = GetComponent<LineRenderer>();
		DrawZone();
	}
	
	void Update ()
	{
		DrawZone();
	}

	void DrawZone()
	{
		Vector3 normal = Planet.GetNormalAtPosition(transform.position);
		Vector3[] linePoses = new Vector3[_numIterations];

		for (int i = 0; i < _numIterations; i++)
		{
			float alpha = i / (float)_numIterations;
			Quaternion offsetRotation = Quaternion.AngleAxis(Mathf.Sin(alpha * Mathf.PI) * 360f, normal);
			Vector3 preAlignPos = transform.position + offsetRotation * transform.right * _zoneRadius;
			linePoses[i] = Planet.GetNearestSurfacePos(preAlignPos);
			linePoses[i] += Planet.GetNormalAtPosition(preAlignPos) * _groundOffset;
		}

		_line.numPositions = _numIterations;
		_line.SetPositions(linePoses);
	}
}
