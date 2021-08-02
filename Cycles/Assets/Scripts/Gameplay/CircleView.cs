using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Kalkatos.Cycles
{
	public class CircleView : MonoBehaviour
	{
		public static Action<float> OnScored; //float score

		[Header("Circle Definition")]
		public float TimeToActivate = 1f;
		public float TimeActive = 0.5f;
		public float Size = 1f;

		private const string emissionColor = "_EmissionColor";

		[Header("Configuration")]
		[SerializeField] private float tolerance;
		[SerializeField] private float finishingFadeTime;
		[SerializeField] private float scoreDecay;
		[SerializeField] private Image thinCircle;
		[SerializeField] private Image thickCircle;
		[SerializeField] private float originalEmissionIntensity;
		[SerializeField] private float pulseEmissionIntensity;
		[SerializeField] private Color originalColor;
		[SerializeField] private Color successColor;
		[SerializeField] private Color failColor;
		[Header("For tutorial")]
		[SerializeField] private bool blockFadingOnMisclick;
		[SerializeField] private bool waitForClick;
		[SerializeField] private float blinkRedTime;

		private string initializationId = "initialization";
		private string pulsatingId = "pulsating";
		private string misclickId = "misclick";
		private string activationId = "activation";
		private Material thickMaterial;
		private int activationState;
		private float activeStartTime;
		private float startTime;
		private bool fading;
		private Color currentStateColor;

		private void Awake ()
		{
			thickMaterial = new Material(thickCircle.material);
			thickCircle.material = thickMaterial;
			initializationId += gameObject.GetInstanceID();
			pulsatingId += gameObject.GetInstanceID();
			misclickId += gameObject.GetInstanceID();
			activationId += gameObject.GetInstanceID();
	}

		private void OnEnable ()
		{
			transform.localScale = Vector3.one * Size;
			activationState = 0;
			ResetColors();
			thinCircle.raycastTarget = true;
			fading = false;
			thickCircle.fillAmount = 0f;
			//thinCircle.DOColor(currentStateColor, TimeToActivate).SetId(initializationId);
			//thickCircle.DOColor(currentStateColor, TimeToActivate).SetId(initializationId);
			DOTween.ToAlpha(() => currentStateColor, (Color c) =>
			{
				currentStateColor = c;
				thinCircle.color = currentStateColor;
				thickCircle.color = currentStateColor;
			}, 1f, TimeToActivate).SetId(initializationId);
			thickCircle.DOFillAmount(1f, TimeToActivate).OnComplete(Activate).SetId(initializationId).SetEase(Ease.Linear);
			startTime = Time.time;
		}

		private void ResetColors ()
		{
			Color fadedColor = originalColor;
			fadedColor.a = 0;
			currentStateColor = originalColor;
			thinCircle.color = fadedColor;
			thickCircle.color = fadedColor;
			thickMaterial.SetVector(emissionColor, originalColor * originalEmissionIntensity);
		}

		private void Activate ()
		{
			activationState = 1;
			activeStartTime = Time.time;
			DOTween.Sequence().AppendInterval(TimeActive).OnComplete(Finish).SetId(activationId);
			//StartCoroutine(DoAfterTime(TimeActive, Finish));
		}

		private void Finish ()
		{
			if (waitForClick)
				transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 1, 1).SetLoops(-1).SetId(pulsatingId);
			else
			{
				activationState = 2;
				Fade(true);
			}
		}

		private void Fade (bool hideThinCircle)
		{
			fading = true;
			thinCircle.color = currentStateColor;
			thickCircle.color = currentStateColor;
			if (hideThinCircle)
			{
				Color fadedColor = originalColor;
				fadedColor.a = 0;
				thinCircle.color = fadedColor;
			}
			DOTween.ToAlpha(() => currentStateColor, (Color c) =>
			{
				currentStateColor = c;
				thinCircle.color = currentStateColor;
				thickCircle.color = currentStateColor;
			}, 0.01f, finishingFadeTime).OnComplete(() => gameObject.SetActive(false));
			DOTween.To(() => thickMaterial.GetVector(emissionColor).w, (float x) => thickMaterial.SetVector(emissionColor, originalColor * x), 1f, finishingFadeTime).SetEase(Ease.InCubic);
		}

		private void Misclick ()
		{
			SetOnlyHue(failColor);
			if (!blockFadingOnMisclick)
			{
				thinCircle.raycastTarget = false;
				Fade(false);
			}
			else
			{
				Color tweeningColor = failColor;
				DOTween.To(() => tweeningColor, (Color c) => {
					tweeningColor = c;
					SetOnlyHue(c);
				}, originalColor, blinkRedTime).SetId(misclickId);
				//thinCircle.DOColor(originalColor, 0.2f).SetId(misclickId);
				//thickCircle.DOColor(originalColor, 0.2f).SetId(misclickId);
			}
			OnScored?.Invoke(0);
		}

		private void SuccessClick ()
		{
			thinCircle.raycastTarget = false;
			thickCircle.fillAmount = 1f;
			SetOnlyHue(successColor);
			float score;
			if (activationState > 0)
			{
				if (activationState == 1)
					thickMaterial.SetVector(emissionColor, originalColor * pulseEmissionIntensity);
				score = Mathf.Clamp01(1 - ((Time.time - activeStartTime) / (TimeActive + finishingFadeTime)));
			}
			else
				score = Mathf.Clamp01(1 - (TimeToActivate - (Time.time - startTime)) / tolerance);
			score *= activationState != 1 ? scoreDecay : 1;
			if (!fading)
				Fade(true);
			OnScored?.Invoke(score);

			Debug.Log("Score: " + (score * 100).ToString("0.00"));
		}

		private void SetOnlyHue (Color color)
		{
			currentStateColor.r = color.r;
			currentStateColor.g = color.g;
			currentStateColor.b = color.b;
		}

		public void OnClick ()
		{
			DOTween.Kill(activationId);
			DOTween.Kill(misclickId);
			DOTween.Kill(pulsatingId);
			if (!blockFadingOnMisclick)
				DOTween.Kill(initializationId);
			if (activationState > 0 || TimeToActivate - (Time.time - startTime) < tolerance)
				SuccessClick();
			else
				Misclick();
		}
	}
}