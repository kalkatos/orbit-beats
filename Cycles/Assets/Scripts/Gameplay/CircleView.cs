using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;
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

		private Material thickMaterial;
		private int activationState;
		private float activeStartTime;
		private float startTime;
		private bool fading;

		private void Awake ()
		{
			thickMaterial = new Material(thickCircle.material);
			thickCircle.material = thickMaterial;
		}

		private void OnEnable ()
		{
			transform.localScale = Vector3.one * Size;
			activationState = 0;
			ResetColors();
			thinCircle.raycastTarget = true;
			fading = false;
			thickCircle.fillAmount = 0f;
			int instanceId = gameObject.GetInstanceID();
			thinCircle.DOColor(originalColor, TimeToActivate).SetId(instanceId);
			thickCircle.DOColor(originalColor, TimeToActivate).SetId(instanceId);
			thickCircle.DOFillAmount(1f, TimeToActivate).OnComplete(Activate).SetId(instanceId).SetEase(Ease.Linear);
			startTime = Time.time;
		}

		private void ResetColors ()
		{
			Color fadedColor = originalColor;
			fadedColor.a = 0;
			thinCircle.color = fadedColor;
			thickCircle.color = fadedColor;
			thickMaterial.SetVector(emissionColor, originalColor * originalEmissionIntensity);
		}

		private void Activate ()
		{
			activationState = 1;
			activeStartTime = Time.time;
			StartCoroutine(DoAfterTime(TimeActive, Finish));
		}

		private void Finish ()
		{
			activationState = 2;
			Fade(true);
		}

		private void Fade (bool hideThinCircle)
		{
			fading = true;
			if (hideThinCircle)
			{
				Color fadedColor = originalColor;
				fadedColor.a = 0.01f;
				thinCircle.color = fadedColor;
			}
			else
				thinCircle.CrossFadeAlpha(0, finishingFadeTime, false);
			thickCircle.CrossFadeAlpha(0, finishingFadeTime, false);
			DOTween.To(() => thickMaterial.GetVector(emissionColor).w, (float x) => thickMaterial.SetVector(emissionColor, originalColor * x), 1f, finishingFadeTime).SetEase(Ease.InCubic);
			StartCoroutine(DoAfterTime(finishingFadeTime, () => gameObject.SetActive(false)));
		}

		private IEnumerator DoAfterTime (float time, Action callback)
		{
			yield return new WaitForSeconds(time);
			callback?.Invoke();
		}

		private void Misclick ()
		{
			thinCircle.raycastTarget = false;
			thinCircle.color = failColor;
			thickCircle.color = failColor;
			Fade(false);
			OnScored?.Invoke(0);
		}

		private void SuccessClick ()
		{
			thinCircle.raycastTarget = false;
			thickCircle.fillAmount = 1f;
			thickCircle.color = successColor;
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

		public void OnClick ()
		{
			DOTween.Kill(gameObject.GetInstanceID());
			if (activationState > 0 || TimeToActivate - (Time.time - startTime) < tolerance)
				SuccessClick();
			else
				Misclick();
		}
	}
}