using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using LinqTools;

public class UIManager : Singleton<UIManager> 
{
	Dictionary<System.Type, List<PanelBase>> _typeToPanelListMap = new Dictionary<System.Type, List<PanelBase>>();

	#region Canvas Properties
	Canvas _worldUICanvas = null;
	public static Canvas GetWorldUICanvas()
	{
		if (!instance._worldUICanvas)
		{
			instance._worldUICanvas = GameObject.Find("WorldUICanvas").GetComponent<Canvas>();
			if (!instance._worldUICanvas)
			{
				Debug.LogError("WorldUICanvas should exist at game start.", instance.gameObject);
				instance._worldUICanvas = new GameObject("WorldUICanvas", typeof(Canvas)).GetComponent<Canvas>();
			}
		}

		return instance._worldUICanvas;
	}

	Canvas _screenUICanvas = null;
	public static Canvas GetScreenUICanvas()
	{
		if (!instance._screenUICanvas)
		{
			instance._screenUICanvas = GameObject.Find("ScreenUICanvas").GetComponent<Canvas>();
			if (!instance._screenUICanvas)
			{
				Debug.LogError("ScreenUICanvas should exist at game start.", instance.gameObject);
				instance._screenUICanvas = new GameObject("ScreenUICanvas", typeof(Canvas)).GetComponent<Canvas>();
			}
		}

		return instance._screenUICanvas;
	}

	GraphicRaycaster _screenRaycaster = null;
	public static GraphicRaycaster GetScreenRaycaster()
	{
		if(!instance._screenRaycaster)
			instance._screenRaycaster = GetScreenRaycaster().GetComponent<GraphicRaycaster>();

		return instance._screenRaycaster;
	}

	RectTransform _screenUIRect = null;
	public static RectTransform GetScreenUIRect()
	{
		if (!instance._screenUIRect)
			instance._screenUIRect = GetScreenUICanvas().GetComponent<RectTransform>();

		return instance._screenUIRect;
	}
	#endregion

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		foreach(PanelBase panel in GetComponentsInChildren<PanelBase>(true))
		{
			if(_typeToPanelListMap.ContainsKey(panel.GetType()))
				_typeToPanelListMap[panel.GetType()].Add(panel);
			else
			{
				List<PanelBase> newPanelList = new List<PanelBase>();
				newPanelList.Add(panel);

				_typeToPanelListMap.Add(panel.GetType(), newPanelList);
			}

			panel.gameObject.SetActive(false);
		}
	}

	public static void Reset()
	{
		// Run through and shut off all UI panels
		foreach(var kvp in instance._typeToPanelListMap)
		{
			foreach (PanelBase panel in kvp.Value)
				panel.gameObject.SetActive(false);
		}
	}

	public static T GetPanel<T>() where T : PanelBase
	{ return (T)instance._typeToPanelListMap[typeof(T)].FirstOrDefault(); }

	public static PanelBase GetTopActivePanel()
	{
		int topSiblingIndex = -1;
		PanelBase topActivePanel = null;
		foreach(var kvp in instance._typeToPanelListMap)
		{
			for(int i = 0; i < kvp.Value.Count; i++)
			{
				PanelBase panel = kvp.Value[i];
				if (panel && panel.gameObject.activeInHierarchy)
				{
					int siblingIndex = panel.transform.GetSiblingIndex();
					if (siblingIndex > topSiblingIndex)
					{
						topActivePanel = panel;
						topSiblingIndex = siblingIndex;
					}
				}
			}
		}

		return topActivePanel;
	}
}
