using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkatos.Cycles
{
	public class ScoreManager : MonoBehaviour
    {
		public static ScoreManager Instance;

		public static Action<double, int> OnScoreWrapUp; //double score, bool positionInLeaderboard (0 = new High Score, -1 = not in Leaderboard)
		public static Action<double> OnScoreChanged;

		public List<double> CurrentLeaderboard => currentLeaderboard;

		[SerializeField] private double scoreMultiplier = 100;
		[SerializeField] private int maxLeaderboardEntries = 6;

        private double currentScore;
		private List<double> currentLeaderboard = new List<double>();

		private void Awake ()
		{
			Instance = this;
			CircleView.OnScored += Scored;
			LevelManager.OnTimelineEnded += TimelineEnded;
		}

		private void Start ()
		{
			LoadLeaderboard();
			if (currentLeaderboard.Count == 0)
				for (int i = maxLeaderboardEntries - 1; i >= 0; i--)
					currentLeaderboard.Add(i + 1);
					//currentLeaderboard.Add(Math.Round(UnityEngine.Random.Range(i * 10 * (float)scoreMultiplier, (i + 1) * 10 * (float)scoreMultiplier)));
		}

		private void OnDestroy ()
		{
			CircleView.OnScored -= Scored;
			LevelManager.OnTimelineEnded -= TimelineEnded;
		}

		private void Scored (float score)
		{
			currentScore += Math.Round(score * scoreMultiplier);
			OnScoreChanged?.Invoke(currentScore);
		}

		private void LoadLeaderboard ()
		{
			currentLeaderboard.Clear();
			string savedLeaderboard = PlayerPrefs.GetString("Leaderboard", "");
			if (!string.IsNullOrEmpty(savedLeaderboard))
			{
				string[] leaderboardBreakdown = savedLeaderboard.Split('|');
				for (int i = 0; i < leaderboardBreakdown.Length; i++)
					currentLeaderboard.Add(double.Parse(leaderboardBreakdown[i]));
			}
		}

		private void TimelineEnded ()
		{
			int scorePosition = -1;
			if (currentScore > currentLeaderboard[currentLeaderboard.Count - 1])
			{
				currentLeaderboard.Add(currentScore);
				currentLeaderboard.Sort(SortLeaderboard);
				if (currentLeaderboard.Count > maxLeaderboardEntries)
					currentLeaderboard.RemoveAt(currentLeaderboard.Count - 1);
				scorePosition = currentLeaderboard.IndexOf(currentScore);

				//Form new leaderboard
				string newLeaderboard = "";
				for (int i = 0; i < currentLeaderboard.Count; i++)
				{
					if (i > 0)
						newLeaderboard += "|";
					newLeaderboard += currentLeaderboard[i].ToString();
				}
				PlayerPrefs.SetString("Leaderboard", newLeaderboard);
			}

			OnScoreWrapUp?.Invoke(currentScore, scorePosition);
		}

		public static int SortLeaderboard (double a, double b)
		{
			if (a > b)
				return -1;
			else if (b > a)
				return 1;
			return 0;
		}
	}
}
