using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkatos.Cycles
{
	public class TutorialManager : MonoBehaviour
	{
		public static bool Finished { get { return PlayerPrefs.GetInt("TutorialFinished", 0) == 1; } }

		[SerializeField] private CanvasGroup tutorial;
		[SerializeField] private CanvasGroup headphonesMessage;
		

		private void Awake ()
		{
			if (Finished)
			{
				gameObject.SetActive(false);
				return;
			}
			else
			{
				tutorial.alpha = 1f;
				headphonesMessage.alpha = 0;
				headphonesMessage.gameObject.SetActive(true);
				Sequence sequence = DOTween.Sequence();
				sequence.AppendInterval(1f);
				sequence.Append(DOTween.To(() => headphonesMessage.alpha, (float x) => headphonesMessage.alpha = x, 1f, 1.5f));
				sequence.AppendInterval(2f);
				sequence.Append(DOTween.To(() => headphonesMessage.alpha, (float x) => headphonesMessage.alpha = x, 0f, 1.5f));
				sequence.Append(DOTween.To(() => tutorial.alpha, (float x) => tutorial.alpha = x, 0f, 2f));
				sequence.Play().OnComplete(EndHeadphonesMessage);
			}
		}

		private void EndHeadphonesMessage ()
		{
			headphonesMessage.gameObject.SetActive(false);
			PlayerPrefs.SetInt("TutorialFinished", 1);
			gameObject.SetActive(false);
		}
	}
}