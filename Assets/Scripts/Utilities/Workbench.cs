using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LinqTools;

public class Workbench : Singleton<Workbench>
{
	void Awake()
	{
		DestroyImmediate(gameObject);
	}
}