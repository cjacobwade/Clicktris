using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sensor<T> : WadeBehaviour where T : Component
{
	// We cache colliders of sensed objects so we can skip getcomponents on things we've already hit
	protected Dictionary<int, T> _idToSensedMap = new Dictionary<int, T>();

	protected List<T> _sensedTs = new List<T>();
	public List<T> GetSensed()
	{
		// This verifies the contents of our sensed list before passing it on
		// Needed for when gameobjects are destroyed while in our sensor
		for(int i = 0; i < _sensedTs.Count; i++)
		{
			if(!_sensedTs[i])
				_sensedTs.RemoveAt(i--);
		}

		return _sensedTs;
	}

	// Subscribe to these callbacks to be notified when something is sensed/unsensed
	public System.Action<T> SensedCallback = delegate{};
	public System.Action<T> UnsensedCallback = delegate{};

	public void Clear()
	{
		_sensedTs.Clear();
	}

	protected virtual void OnTriggerEnter(Collider collider)
	{
		int id = collider.GetInstanceID();
		if (collider.attachedRigidbody)
			id = collider.attachedRigidbody.GetInstanceID();

		T hitT = null;

		bool hasCachedT = _idToSensedMap.TryGetValue(id, out hitT);
		if (!hasCachedT)
		{
			if(collider.attachedRigidbody)
				hitT = collider.attachedRigidbody.GetComponent<T>();
			else
				hitT = collider.GetComponent<T>();

			_idToSensedMap.Add(id, hitT);
		}

		if(hitT && !_sensedTs.Contains(hitT))
		{
			_sensedTs.Add(hitT);
			SensedCallback(hitT);
		}
	}

	protected virtual void OnTriggerExit(Collider collider)
	{
		int id = collider.GetInstanceID();
		if (collider.attachedRigidbody)
			id = collider.attachedRigidbody.GetInstanceID();

		T unhitT = null;

		_idToSensedMap.TryGetValue(id, out unhitT);
		if (unhitT != null)
		{
			_sensedTs.Remove(_idToSensedMap[id]);
			UnsensedCallback(unhitT);
		}
	}

	protected virtual void OnDisable()
	{
		for(int i = 0; i < _sensedTs.Count; i++)
			UnsensedCallback(_sensedTs[i]);

		_sensedTs.Clear();
	}
}
