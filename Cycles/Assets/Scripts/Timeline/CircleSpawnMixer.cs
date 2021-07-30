using UnityEngine;
using UnityEngine.Playables;

namespace Kalkatos.Cycles
{
	public class CircleSpawnMixer : PlayableBehaviour
	{
		public override void OnGraphStart (Playable playable)
		{
			base.OnGraphStart(playable);
			Debug.Log("CircleSpawnMixer Started");
		}

		public override void OnGraphStop (Playable playable)
		{
			base.OnGraphStop(playable);
			Debug.Log("CircleSpawnMixer Stopped");
		}
	}
}
