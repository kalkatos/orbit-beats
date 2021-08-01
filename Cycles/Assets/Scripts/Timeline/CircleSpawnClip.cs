using UnityEngine;
using UnityEngine.Playables;

namespace Kalkatos.Cycles
{
	public class CircleSpawnClip : PlayableAsset
	{
		public int scoringPerCircle;

		public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
		{
			return ScriptPlayable<CircleSpawnBehaviour>.Create(graph);
		}
	}
}
