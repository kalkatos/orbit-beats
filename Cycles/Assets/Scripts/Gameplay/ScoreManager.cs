using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkatos.Cycles
{
	public class ScoreManager : MonoBehaviour
    {
        public static Action<double> OnScoreChanged;

		[SerializeField] private double scoreMultiplier = 100;

        private double currentScore;

		private void Awake ()
		{
			CircleView.OnScored += Scored;
		}

		private void OnDestroy ()
		{
			CircleView.OnScored -= Scored;
		}

		private void Scored (float score)
		{
			currentScore += Math.Round(score * scoreMultiplier);
			OnScoreChanged?.Invoke(currentScore);
		}
	}
}
