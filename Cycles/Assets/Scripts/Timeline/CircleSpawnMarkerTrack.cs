using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;

namespace Kalkatos.Cycles
{
	[TrackBindingType(typeof(CircleSpawner))]
	[TrackClipType(typeof(CircleSpawnClip))]
	public class CircleSpawnMarkerTrack : MarkerTrack
	{
		public bool isCreatingMarks;

		private void OnEnable ()
		{
			CircleSpawnMixer.OnMouseClickWhilePlaying += MouseClick;
		}

		private void OnDisable ()
		{
			CircleSpawnMixer.OnMouseClickWhilePlaying -= MouseClick;
		}

		private void MouseClick (Vector2 position, double time)
		{
			if (!muted && isCreatingMarks)
			{
				CircleSpawnMarker newMarker = CreateMarker<CircleSpawnMarker>(time - 1);
				newMarker.Position = GameVariables.GetScreenPercent(position);
			}
		}

		public override Playable CreateTrackMixer (PlayableGraph graph, GameObject go, int inputCount)
		{
			return ScriptPlayable<CircleSpawnMixer>.Create(graph, inputCount);
		}
	}
}