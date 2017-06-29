#if UNITY_EDITOR
using UnityEditor;
using System.Diagnostics;
using System.Reflection;
#endif

using UnityEngine;
using System.Collections.Generic;
using UObject = UnityEngine.Object;

public class DebugTracer : MonoBehaviour
{
	#pragma warning disable 0219
	
	[SerializeField]
	string _stackTrace;
	
	[SerializeField]
	string _time;

	[SerializeField]
	int _frame;

	#pragma warning restore 0219

	void Awake()
	{
		#if UNITY_EDITOR

		// Get call stack and the calling method.
		StackTrace stackTrace = new StackTrace(4, true);
		_stackTrace = stackTrace.ToString();

		// Get time and frame.
		_time = System.DateTime.Now.ToString("HH:mm:ss:FFFF");
		_frame = Time.frameCount;

		#endif
	}

	public override string ToString()
	{
		return "<color=lime>F(" + _frame + "), T(" + _time + ")</color>\n" + _stackTrace;
	}
}
