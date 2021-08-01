using System;
using UnityEngine;

namespace Kalkatos.Cycles
{
	[DefaultExecutionOrder(-1)]
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;

		public static Action OnTimelineEnded;

		private ScoreManager scoreManager;

		private void Awake ()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			scoreManager = GetComponent<ScoreManager>();
		}

		public void EndOfTimeline ()
		{
			OnTimelineEnded?.Invoke();
		}
	}
}
