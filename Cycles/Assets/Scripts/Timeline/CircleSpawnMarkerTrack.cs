using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Kalkatos.Cycles
{
	[TrackBindingType(typeof(CircleSpawner))]
	[TrackClipType(typeof(CircleSpawnMarker))]
	public class CircleSpawnMarkerTrack : MarkerTrack
	{
	}
}