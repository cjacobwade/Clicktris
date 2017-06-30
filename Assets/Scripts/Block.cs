using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : WadeBehaviour
{
	void Awake()
	{
		Animator[] blockAnims = GetComponentsInChildren<Animator>();
		for(int i = 0; i < blockAnims.Length; i++)
			blockAnims[i].Play(string.Format("BlockJiggle{0}", Random.Range(1, 4)), 0, Random.value);
	}
}
