using System;
using UnityEngine;

namespace Kalkatos.Cycles
{
	public class LevelManager : MonoBehaviour
	{
		public static Action OnTimelineEnded;

		public void EndOfTimeline ()
		{
			OnTimelineEnded?.Invoke();
		}

		public void LoadNextLevel ()
		{

		}
	}
}
