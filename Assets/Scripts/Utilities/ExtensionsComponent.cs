using UnityEngine;
using LinqTools;
using System.Collections.Generic;
using UObject = UnityEngine.Object;

public static class ExtensionsComponent
{
	//
	// AddComponent directly from other component
	//

	public static T AddComponent<T>(this Component component) where T : Component
	{
		Debug.Assert(component != null);
		return component.gameObject.AddComponent<T>();
	}

	//
	// Multiple get.
	//
	public static T[] GetComponents<T>(this GameObject gameObject, bool includeInactive) where T : Component
	{
		return gameObject.transform.GetComponents<T>(includeInactive);
	}

	public static T[] GetComponents<T>(this Component component, bool includeInactive) where T : Component
	{
		T[] tArray = component.GetComponentsInChildren<T>(includeInactive);

		List<T> results = new List<T>();
		for(int i = 0; i < tArray.Length; i++)
		{
			T t = tArray[i];
			if(t != null && t.transform == component.transform)
				results.Add(t);
		}
		return results.ToArray();
	}

	//
	// Single get, include inactive
	//

	public static T GetComponent<T>(this Component component, bool includeInactive) where T : Component
	{
		Debug.Assert(component != null);
		return component.gameObject.GetComponent<T>(includeInactive);
	}

	public static T GetComponent<T>(this GameObject gameObject, bool includeInactive) where T : Component
	{
		var tReferences = gameObject.GetComponentsInChildren<T>(true);
		foreach(var t in tReferences)
		{
			if(t.transform == gameObject.transform)
				return t;
		}
		return default(T);
	}

	public static T GetComponentInChildren<T>(this Component component, bool includeInactive) where T : Component
	{
		Debug.Assert(component != null);
		return component.gameObject.GetComponentInChildren<T>(includeInactive);
	}

	public static T GetComponentInChildren<T>(this GameObject gameObject, bool includeInactive) where T : Component
	{
		T[] tArray = gameObject.GetComponentsInChildren<T>(includeInactive);
		if(tArray != null && tArray.Length > 0)
		{
			return tArray[0];
		}
		return default(T);
	}

	//
	// Immediate children
	//

	public static T[] GetComponentsInImmediateChildren<T>(this Component component) where T : Component
	{
		Debug.Assert(component != null);
		return component.gameObject.GetComponentsInImmediateChildren<T>();
	}

	public static T[] GetComponentsInImmediateChildren<T>(this GameObject gameObject) where T : Component
	{
		Debug.Assert(gameObject != null);

		List<T> results = new List<T>();
		foreach(Transform child in gameObject.transform)
		{
			T found = child.GetComponent<T>();
			if(found != null)
				results.Add(found);
		}

		return results.ToArray();
	}

	public static T GetComponentInImmediateChildren<T>(this Component component) where T : Component
	{
		Debug.Assert(component != null);
		return component.gameObject.GetComponentInImmediateChildren<T>();
	}

	public static T GetComponentInImmediateChildren<T>(this GameObject gameObject) where T : Component
	{
		Debug.Assert(gameObject != null);

		foreach(Transform child in gameObject.transform)
		{
			T found = child.GetComponent<T>();
			if(found != null)
				return found;
		}

		return default(T);
	}

	//
	// Ancestors, generic
	//

	public static T GetComponentInAncestors<T>(this GameObject gameObject, bool includeInactive = false, Component inSearchTerminal = null) where T : Component
	{
		Debug.Assert(gameObject != null);
		return gameObject.transform.GetComponentInAncestors<T>(includeInactive, inSearchTerminal);
	}

	public static T GetComponentInAncestors<T>(this Component component, bool includeInactive = false, Component inSearchTerminal = null) where T : Component
	{
		Debug.Assert(component != null);

		T returnComponent = null;
		Transform t = component.transform;
		while(t != null)
		{
			returnComponent = t.GetComponent<T>(includeInactive);
			if(returnComponent != null)
				break;

			if(inSearchTerminal)
			{
				var allComponents = t.GetComponents<Component>(includeInactive);
				if(allComponents.Contains(inSearchTerminal))
					break;
			}

			t = t.parent;
		}
		return returnComponent;
	}

	public static T[] GetComponentsInAncestors<T>(this GameObject gameObject, bool includeInactive = false, Component inSearchTerminal = null) where T : Component
	{
		return gameObject.transform.GetComponentsInAncestors<T>(includeInactive, inSearchTerminal);
	}

	public static T[] GetComponentsInAncestors<T>(this Component component, bool includeInactive = false, Component inSearchTerminal = null) where T : Component
	{
		List<T> returnComponents = new List<T>();

		Transform t = component.transform;

		while(t != null)
		{
			var ancestorComponents = t.GetComponents<T>(includeInactive);
			if(ancestorComponents.Length > 0)
			{
				returnComponents.AddRange(ancestorComponents);
			}

			if(inSearchTerminal)
			{
				var allComponents = t.GetComponents<Component>(includeInactive);
				if(allComponents.Contains(inSearchTerminal))
					break;
			}

			t = t.parent;
		}
		return returnComponents.ToArray();
	}

	//
	// Ancestors, non generic
	//

	public static Component GetComponentInAncestors(this GameObject gameObject, System.Type componentType)
	{
		return gameObject.transform.GetComponentInAncestors(componentType);
	}

	public static Component GetComponentInAncestors(this Component component, System.Type componentType)
	{
		Component returnComponent = null;
		Transform t = component.transform;
		while(t != null)
		{
			returnComponent = t.GetComponent(componentType);
			if(returnComponent != null)
				break;
			t = t.parent;
		}
		return returnComponent;
	}

	public static Component[] GetComponentsInAncestors(this GameObject gameObject, System.Type componentType)
	{
		return gameObject.transform.GetComponentsInAncestors(componentType);
	}

	public static Component[] GetComponentsInAncestors(this Component component, System.Type componentType)
	{
		List<Component> returnComponents = new List<Component>();
		Transform t = component.transform;
		while(t != null)
		{
			Component ancestorComponent = t.GetComponent(componentType);
			if(ancestorComponent != null)
			{
				returnComponents.Add(ancestorComponent);
			}
			t = t.parent;
		}
		return returnComponents.ToArray();
	}

	// Oldest ancestor

	public static T GetOldestAncestor<T>(this Component component) where T : Component
	{
		T oldestComponent = null;
		Transform t = component.transform;
		while(t != null)
		{
			T potentialComponent = t.GetComponent<T>();
			if(potentialComponent != null)
			{
				oldestComponent = potentialComponent;
			}
			t = t.parent;
		}
		return oldestComponent;
	}

	public static T GetOldestAncestor<T>(this GameObject gameObject) where T : Component
	{
		return gameObject.transform.GetOldestAncestor<T>();
	}

	// Component in any direction

	public static T GetComponentInChildrenOrAncestors<T>(this Component component) where T : Component
	{
		T target = component.GetComponentInChildren<T>();
		if(target == null)
			target = component.GetComponentInAncestors<T>();
		return target;
	}

	public static T GetComponentInChildrenOrAncestors<T>(this GameObject gameObject) where T : Component
	{
		return gameObject.transform.GetComponentInChildrenOrAncestors<T>();
	}

	// Siblings

	public static T[] GetSiblings<T>(this Component component) where T : Component
	{
		Transform parent = component.transform.parent;
		if(parent == null) return new T[0];

		List<T> siblings = new List<T>();
		foreach(Transform child in parent)
		{
			// Meaning a gameObject with two components of the same type.
			T[] conjoinedSiblings = child.GetComponents<T>();
			if(conjoinedSiblings.Length > 0)
				siblings.AddRange(conjoinedSiblings);
		}

		return siblings.ToArray();
	}

	public static T[] GetSiblings<T>(this GameObject gameObject) where T : Component
	{
		return gameObject.transform.GetSiblings<T>();
	}

	// Interface methods from: 
	// http://forum.unity3d.com/threads/101028-How-to-Get-all-components-on-an-object-that-implement-an-interface

	public static T[] GetInterfaces<T>(this Component inComponent)
	{ return inComponent.gameObject.GetInterfaces<T>(); }

	public static T[] GetInterfaces<T>(this GameObject inGameObject)
	{
		if(!UtilitiesReflection.IsInterface(typeof(T)))
			throw new System.Exception("Specified type is not an interface!");

		MonoBehaviour[] monoBehaviours = inGameObject.GetComponents<MonoBehaviour>();
		return monoBehaviours.Where(mb => mb != null && UtilitiesReflection.GetInterfaces(mb.GetType()).Any(iType => iType == typeof(T)))
			.Select(mb => (T)(object)mb)
			.ToArray();
	}

	public static void GetInterfacesNonAlloc<T>(this GameObject inGameObject, ref List<T> outList)
	{
		if (!UtilitiesReflection.IsInterface(typeof(T)))
			throw new System.Exception("Specified type is not an interface!");

		outList.Clear();

		MonoBehaviour[] monoBehaviours = inGameObject.GetComponents<MonoBehaviour>();
		for (int i = 0; i < monoBehaviours.Length; i++)
		{
			MonoBehaviour mb = monoBehaviours[i];
			if (mb)
			{
				IEnumerator<System.Type> types = UtilitiesReflection.GetInterfaces(mb.GetType()).GetEnumerator();
				while (types.MoveNext())
				{
					if (types.Current == typeof(T))
						outList.Add((T)(object)mb);
				}
			}
		}
	}

	public static T GetInterface<T>(this Component inComponent)
	{ return inComponent.gameObject.GetInterface<T>(); }

	public static T GetInterface<T>(this GameObject inGameObject, bool includeInactive = false)
	{
		if (!UtilitiesReflection.IsInterface(typeof(T)))
			throw new System.Exception("Specified type is not an interface!");

		MonoBehaviour[] monoBehaviours = inGameObject.GetComponents<MonoBehaviour>(includeInactive);
		for (int i = 0; i < monoBehaviours.Length; i++)
		{
			MonoBehaviour mb = monoBehaviours[i];
			if (mb)
			{
				IEnumerator<System.Type> types = UtilitiesReflection.GetInterfaces(mb.GetType()).GetEnumerator();
				while (types.MoveNext())
				{
					if (types.Current == typeof(T))
						return (T)(object)mb;
				}
			}
		}

		return default(T);
	}

	public static T GetInterfaceInChildren<T>(this Component inComponent)
	{ return inComponent.gameObject.GetInterfaceInChildren<T>(); }

	public static T GetInterfaceInChildren<T>(this GameObject inGameObject, bool includeChildren = false)
	{
		if (!UtilitiesReflection.IsInterface(typeof(T)))
			throw new System.Exception("Specified type is not an interface!");

		MonoBehaviour[] monoBehaviours = inGameObject.GetComponentsInChildren<MonoBehaviour>(includeChildren);
		for (int i = 0; i < monoBehaviours.Length; i++)
		{
			MonoBehaviour mb = monoBehaviours[i];
			if (mb)
			{
				IEnumerator<System.Type> types = UtilitiesReflection.GetInterfaces(mb.GetType()).GetEnumerator();
				while (types.MoveNext())
				{
					if (types.Current == typeof(T))
						return (T)(object)mb;
				}
			}
		}

		return default(T);
	}

	public static T[] GetInterfacesInChildren<T>(this Component inComponent)
	{ return inComponent.gameObject.GetInterfacesInChildren<T>(); }

	public static T[] GetInterfacesInChildren<T>(this GameObject inGameObject)
	{
		if(!UtilitiesReflection.IsInterface(typeof(T)))
			throw new System.Exception("Specified type is not an interface!");

		var monoBehaviours = inGameObject.GetComponentsInChildren<MonoBehaviour>(true);
		return monoBehaviours.Where(mb => mb != null && UtilitiesReflection.GetInterfaces(mb.GetType()).Any(iType => iType == typeof(T)))
			.Select(mb => (T)(object)mb)
			.ToArray();
	}

	public static void GetInterfacesInChildrenNonAlloc<T>(this GameObject inGameObject, ref List<T> outList, bool includeChildren = false)
	{
		if (!UtilitiesReflection.IsInterface(typeof(T)))
			throw new System.Exception("Specified type is not an interface!");

		outList.Clear();

		MonoBehaviour[] monoBehaviours = inGameObject.GetComponentsInChildren<MonoBehaviour>(includeChildren);
		for (int i = 0; i < monoBehaviours.Length; i++)
		{
			MonoBehaviour mb = monoBehaviours[i];
			if (mb)
			{
				IEnumerator<System.Type> types = UtilitiesReflection.GetInterfaces(mb.GetType()).GetEnumerator();
				while (types.MoveNext())
				{
					if (types.Current == typeof(T))
						outList.Add((T)(object)mb);
				}
			}
		}
	}

	public static T GetInterfaceInAncestors<T>(this GameObject inGameObject)
	{ return inGameObject.transform.GetInterfaceInAncestors<T>(); }

	public static T GetInterfaceInAncestors<T>(this Component inComponent)
	{
		T returnInterface = default(T);

		Transform t = inComponent.transform;
		while(t != null)
		{
			returnInterface = t.GetInterface<T>();
			if(returnInterface != null)
				break;
			t = t.parent;
		}
		return returnInterface;
	}

	// Get OR add

	public static T GetOrAddComponent<T>(this Component component) where T : Component
	{
		return component.gameObject.GetOrAddComponent<T>();
	}

	public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
	{
		T t = gameObject.GetComponent<T>();
		if(t == null)
		{
			t = gameObject.AddComponent<T>();
		}
		return t;
	}

	// Destroy

	public static T DestroyComponent<T>(this GameObject gameObject) where T : Component
	{
		return gameObject.transform.DestroyComponent<T>();
	}

	public static T DestroyComponent<T>(this Component component) where T : Component
	{
		T t = component.GetComponent<T>();
		if(t != null)
			UnityEngine.Object.Destroy(t);
		return t;
	}
}
