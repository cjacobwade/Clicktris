using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAudio : WadeBehaviour
{
	[SerializeField]
	GameSound[] _startSounds = null;

	void Start()
	{
		for (int i = 0; i < _startSounds.Length; i++)
			GameSounds.PostMusicEvent(_startSounds[i]);
	}

	void OnDestroy()
	{
		for (int i = 0; i < _startSounds.Length; i++)
			GameSounds.PostMusicEvent(_startSounds[i], Fabric.EventAction.StopSound);
	}
}
