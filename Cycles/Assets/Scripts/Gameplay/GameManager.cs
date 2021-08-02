using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kalkatos.Cycles
{
	[DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;

		public static int CurrentLevel;

		private void Awake ()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			LevelManager.OnTimelineEnded += EndOfTimeline;
		}

		private void OnDestroy ()
		{
			LevelManager.OnTimelineEnded -= EndOfTimeline;
		}

		private void EndOfTimeline ()
		{
			PlayerPrefs.SetInt("LastLevel", CurrentLevel + 1);
		}

		public static void LoadLevel (int number)
		{
			CurrentLevel = number;
			SceneManager.LoadScene("Level" + number);
		}

		public static void LoadLevelSelector ()
		{
			SceneManager.LoadScene("LevelSelector");
			
		}
	}
}
