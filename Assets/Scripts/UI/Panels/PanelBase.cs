using UnityEngine;
using System.Collections;
using LinqTools;
using System.Collections.Generic;

public class PanelBase : WadeBehaviour 
{
	protected Coroutine _showRoutine = null;

	public virtual bool CanInterrupt()
	{ return false; }

	public bool CanShow()
	{ return CanInterrupt() || _showRoutine == null; }

	[ContextMenu("Show Panel", true)]
	bool ShowValidate()
	{ return CanShow(); }

	// Call from a panel's enable for easy debugging
	[ContextMenu("Show Panel")]
	protected Coroutine Show()
	{ return Show(true); }

	[ContextMenu("Hide Panel", true)]
	bool HideValidate()
	{ return CanShow(); }

	[ContextMenu("Hide Panel")]
	public Coroutine Hide()
	{ return Show(false); }

	Coroutine Show(bool show)
	{
		if (_showRoutine != null)
			StopCoroutine(_showRoutine);

		gameObject.SetActive(true);

		_showRoutine = StartCoroutine(show ? ShowRoutine() : HideRoutine());
		return _showRoutine;
	}

	public virtual IEnumerator ShowRoutine()
	{
		_showRoutine = null;
		yield break;
	}

	public virtual IEnumerator HideRoutine()
	{
		gameObject.SetActive(false);
		_showRoutine = null;
		yield break;
	}

	public bool IsTopActivePanel()
	{ return this == UIManager.GetTopActivePanel(); }
}
