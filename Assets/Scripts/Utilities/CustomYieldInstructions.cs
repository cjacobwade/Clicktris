using UnityEngine;
using System.Collections;
using System;

public class WaitForSecondsRealtime : CustomYieldInstruction
{
	float _startTime = 0f;
	float _waitTime = 0f;

	public WaitForSecondsRealtime(float realtimeSeconds)
	{
		_startTime = Time.realtimeSinceStartup;
		_waitTime = realtimeSeconds;
	}

	public override bool keepWaiting
	{ get { return Time.realtimeSinceStartup - _startTime < _waitTime; } }
}