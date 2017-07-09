using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : PanelBase
{
	[SerializeField]
	ScrollRect _scrollRect = null;

	[SerializeField]
	RectTransform _itemLayout = null;

	ShopItemWidget _itemWidgetTemplate = null;

	List<ShopItemWidget> _itemWidgets = new List<ShopItemWidget>();

	// TODO: Items should be loaded from resources probably?

	void Awake()
	{
		_scrollRect.normalizedPosition = Vector2.up;

		_itemWidgetTemplate = GetComponentInChildren<ShopItemWidget>(true);
		_itemWidgetTemplate.gameObject.SetActive(false);
	}

	void SetupItems(List<ShopItemWidget> widgetPrefabs)
	{
		for(int i = 0; i < widgetPrefabs.Count; i++)
		{
			ShopItemWidget spawnedWidget = Instantiate<ShopItemWidget>(widgetPrefabs[i], _itemLayout);
			_itemWidgets.Add(spawnedWidget);
		}
	}
}