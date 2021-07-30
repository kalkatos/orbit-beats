using UnityEngine;
using UnityEngine.Playables;

namespace Kalkatos.Cycles
{
	public class CircleSpawnClip : PlayableAsset
	{
		public int scoringPerCircle;

		public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
		{
			var playable = ScriptPlayable<CircleSpawnBehaviour>.Create(graph);

			CircleSpawnBehaviour circleSpawnBehaviour = playable.GetBehaviour();
			circleSpawnBehaviour.scoringPerCircle = scoringPerCircle;

			return playable;
		}
	}
}
