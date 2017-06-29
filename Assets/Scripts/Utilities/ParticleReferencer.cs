using UnityEngine;
using System.Collections;

public class ParticleReferencer : WadeBehaviour
{
	[SerializeField]
	ParticleSystem[] _psArray = null;
	public ParticleSystem[] particleSystems
	{ get { return _psArray; } }

	[SerializeField]
	ParticleSystemRenderer[] _psRenderers = null;
	public ParticleSystemRenderer[] particleSystemRenderers
	{ get { return _psRenderers; } }
}
