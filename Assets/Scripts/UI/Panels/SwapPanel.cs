using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPanel : PanelBase
{
	public void CloseAll()
	{
		UIManager.GetPanel<BreedPanel>().gameObject.SetActive(false);
		UIManager.GetPanel<PowerupPanel>().gameObject.SetActive(false);
		UIManager.GetPanel<ShopPanel>().gameObject.SetActive(false);
		UIManager.GetPanel<CodexPanel>().gameObject.SetActive(false);
	}

	public void OnClickBreed()
	{
		CloseAll();
		UIManager.GetPanel<BreedPanel>().gameObject.SetActive(true);
	}

	public void OnClickPowerup()
	{
		CloseAll();
		UIManager.GetPanel<PowerupPanel>().gameObject.SetActive(true);
	}

	public void OnClickShop()
	{
		CloseAll();
		UIManager.GetPanel<ShopPanel>().gameObject.SetActive(true);
	}

	public void OnClickCodex()
	{
		CloseAll();
		UIManager.GetPanel<CodexPanel>().gameObject.SetActive(true);
	}
}