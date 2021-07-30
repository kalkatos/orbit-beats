using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Kalkatos.Cycles
{
	[TrackBindingType(typeof(CircleSpawner))]
	[TrackClipType(typeof(CircleSpawnClip))]
	public class CircleSpawnMarkerTrack : MarkerTrack
	{
		public override Playable CreateTrackMixer (PlayableGraph graph, GameObject go, int inputCount)
		{
			return ScriptPlayable<CircleSpawnMixer>.Create(graph, inputCount);
		}
	}
}