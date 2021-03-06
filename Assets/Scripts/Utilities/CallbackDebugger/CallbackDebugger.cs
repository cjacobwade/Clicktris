using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using System.Diagnostics;
#endif

// Put this component on a class to report when Unity callbacks are happening.
// Useful in the debugger for determining what is disabling the object.
public class CallbackDebugger : MonoBehaviour
{
	[SerializeField]
	bool _useComponentTracers = false;

	[SerializeField]
	bool _awake = false;

	[SerializeField]
	bool _start = false;

	[SerializeField]
	bool _onEnable = false;

	[SerializeField]
	bool _onDisable = false;

	[SerializeField]
	bool _onDestroy = false;

	[SerializeField]
	bool _onCollideEnter = false;

	void Awake()
	{
		if(_awake)
		{
			if(_useComponentTracers)
				gameObject.AddComponent<DebugTracer>();
			else
				Trace();
		}
	}
	
	void Start()
	{
		if(_start)
		{
			if(_useComponentTracers)
				gameObject.AddComponent<DebugTracer>();
			else
				Trace();
		}
	}
	
	void OnEnable()
	{
		if(_onEnable)
		{
			if(_useComponentTracers)
				gameObject.AddComponent<DebugTracer>();
			else
				Trace();
		}
	}
	
	void OnDisable()
	{
		if(_onDisable)
		{
			if(_useComponentTracers)
				gameObject.AddComponent<DebugTracer>();
			else
				Trace();
		}
	}

	void OnCollisionEnter(Collision inCollision)
	{
		if(_onCollideEnter)
		{
			if(_useComponentTracers)
				gameObject.AddComponent<DebugTracer>();
			else
				Trace();
		}
	}
	
	void OnDestroy()
	{
		if(_onDestroy)
			Trace();
	}

	void Trace()
	{
#if UNITY_EDITOR

		// Get call stack and the calling method.
		StackTrace stackTrace = new StackTrace(true);

		// Don't report this call, instead report the caller.
		StackFrame stackFrame = stackTrace.GetFrame(1);

		// Print the calling function.
		UnityEngine.Debug.Log(stackFrame.ToString(), this);
#endif
	}
}