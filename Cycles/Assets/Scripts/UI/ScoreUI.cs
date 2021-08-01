using UnityEngine;
using TMPro;
using System;

namespace Kalkatos.Cycles
{
	public class ScoreUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI scoreText;

		private void Awake ()
		{
			ScoreManager.OnScoreChanged += ChangeScore;
		}

		private void OnDestroy ()
		{
			ScoreManager.OnScoreChanged -= ChangeScore;
		}

		private void ChangeScore (double score)
		{
			scoreText.text = score.ToString();
		}
	}
}
