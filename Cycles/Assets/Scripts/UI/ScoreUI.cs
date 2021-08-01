using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

namespace Kalkatos.Cycles
{
	public class ScoreUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI scoreText;

		private double currentScore;

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
			DOTween.Kill(gameObject.GetInstanceID());
			double newScore = currentScore;
			DOTween.To(() => newScore, (double x) => 
			{
				newScore = x;
				scoreText.text = Math.Round(newScore).ToString();
			}, score, 0.5f).SetId(gameObject.GetInstanceID());
			scoreText.text = score.ToString();
			currentScore = score;
		}
	}
}
