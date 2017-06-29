using UnityEngine;
using UnityEngine.UI;
using LinqTools;

[RequireComponent(typeof(GridLayoutGroup)), ExecuteInEditMode]
public class GridLayoutDynamicSize : WadeBehaviour
{
	GridLayoutGroup _gridLayoutGroup = null;
	GridLayoutGroup gridLayoutGroup
	{
		get
		{
			if (!_gridLayoutGroup)
				_gridLayoutGroup = GetComponent<GridLayoutGroup>();

			return _gridLayoutGroup;
		}
	}

	[SerializeField]
	int _numRows = 1;

	[SerializeField]
	int _numColumns = 1;

	int _childCount = 0;
	int _cellCount = 0;

	void OnEnable()
	{
		UpdateCellCount();	
	}

	void UpdateCellCount()
	{
		_cellCount = 0;
		_childCount = transform.childCount;

		foreach(Transform child in transform)
		{
			LayoutElement element = child.GetComponent<LayoutElement>();
			if (!element || element.ignoreLayout)
				_cellCount++;
		}
	}

	void LateUpdate()
	{
		UpdateCellSize();
	}

	protected void OnRectTransformDimensionsChange()
	{
		UpdateCellSize();
	}

	void UpdateCellSize()
	{
		if (_cellCount == 0 || transform.childCount != _childCount)
			UpdateCellCount();

		float xPadding = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
		float yPadding = gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;

		float xSplits = (float)(_numColumns - 1);
		float ySplits = (float)(_numRows - 1);

		Vector2 cellSize = Vector2.zero;
		cellSize.x = rectTransform.rect.width/(float)_numColumns - (xPadding + gridLayoutGroup.spacing.x * xSplits)/(float)_numColumns;
		cellSize.y = rectTransform.rect.height/(float)_numRows - (yPadding + gridLayoutGroup.spacing.y * ySplits)/(float)_numRows;
		gridLayoutGroup.cellSize = cellSize;
	}
}
