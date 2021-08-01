using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Kalkatos.Cycles
{
	public class CircleSpawnMixer : PlayableBehaviour
	{
		public static Action<Vector2, double> OnMouseClickWhilePlaying;
		public Playable currentPlayable;

		private void MouseRightClickInGame (Vector2 position)
		{
			//Debug.Log("Detected a mouse click at: " + position);
			OnMouseClickWhilePlaying?.Invoke(position, currentPlayable.GetTime());
		}

		public override void OnGraphStart (Playable playable)
		{
			currentPlayable = playable;
			//Debug.Log("CircleSpawnBehaviour Started");
			PointDetector.OnMouseRightClick += MouseRightClickInGame;
		}

		public override void OnGraphStop (Playable playable)
		{
			//Debug.Log("CircleSpawnBehaviour Stopped");
			PointDetector.OnMouseRightClick -= MouseRightClickInGame;
		}
	}
}
