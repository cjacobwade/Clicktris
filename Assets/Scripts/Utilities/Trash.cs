using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Trash : Singleton<Trash>
{
	public static Transform Pile
	{ get { return instance.transform; } }

	void Awake()
	{
		SceneManager.sceneLoaded += OnLoad;
	}

	public static void TakeOut()
	{
		Transform[] children = instance.transform.GetComponentsInChildren<Transform>();
		for (int i = 0; i < children.Length; i++)
		{
			if (children[i] && children[i] != instance.transform)
				Destroy(children[i].gameObject);
		}
	}

	void OnLoad(Scene scene, LoadSceneMode loadMode)
	{
		TakeOut();
	}

	void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnLoad;
	}
}
