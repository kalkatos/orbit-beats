using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

namespace Kalkatos.Cycles
{
	public class EndScreen : MonoBehaviour
	{
		[SerializeField] private GameObject containerObj;
		[SerializeField] private GameObject newHighscoreObj;
		[SerializeField] private TextMeshProUGUI currentScoreText;
		[SerializeField] private TextMeshProUGUI leaderboardText;

		private void Awake ()
		{
			ScoreManager.OnScoreWrapUp += ScoreWrapUp;

			containerObj.SetActive(false);
			newHighscoreObj.SetActive(false);
		}

		private void OnDestroy ()
		{
			ScoreManager.OnScoreWrapUp -= ScoreWrapUp;
		}

		private void ScoreWrapUp (double currentScore, int positionInLeaderboard)
		{
			containerObj.SetActive(true);
			currentScoreText.text = currentScore.ToString();
			newHighscoreObj.SetActive(positionInLeaderboard == 0);
			List<double> leaderboard = ScoreManager.Instance.CurrentLeaderboard;
			string leaderboardString = "";
			for (int i = 0; i < leaderboard.Count; i++)
			{
				bool addMarkings = positionInLeaderboard >= 0 && positionInLeaderboard == i;
				if (addMarkings)
					leaderboardString += ">> ";
				leaderboardString += $"{i + 1}. {leaderboard[i]}";
				if (addMarkings)
					leaderboardString += " <<";
				leaderboardString += Environment.NewLine;
			}
			leaderboardText.text = leaderboardString;
		}
	}
}
