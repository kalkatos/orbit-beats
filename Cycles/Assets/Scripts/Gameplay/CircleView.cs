using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;

namespace Kalkatos.Cycles
{
	public class CircleView : MonoBehaviour, IPointerClickHandler
	{
		public static Action OnCircleStarted;
		public static Action<float> OnCircleCompleted; //float score

		#region Fields

		public float MaxTime = 3f;
		public float TargetRadius = 1f;
		public float SpeedWeight = 1f;
		public float AccuracyWeight = 1f;
		public bool TurnOffWhenEnded = true;
		public UnityEvent OnGrowFinishedEvent;
		public UnityEvent OnClickedEvent;

		[SerializeField] private Renderer myRenderer;
		[SerializeField] private float originalEmissionIntensity;
		[SerializeField] private float pulseEmissionIntensity;
		[SerializeField] private Color originalColor;
		[SerializeField] private Color successColor;
		[SerializeField] private Color failColor;

		private const string emissionColor = "_EmissionColor";

		private float startTime;
		private Material myMaterial;
		private Collider2D myCollider;

		#endregion

		private void Awake ()
		{
			myMaterial = myRenderer.material;
			myCollider = GetComponent<Collider2D>();
			ResetColors();
		}

		private void OnEnable ()
		{
			transform.localScale = Vector3.one * 0.1f;
			transform.DOScale(TargetRadius, MaxTime).OnComplete(GrowEnded).SetEase(Ease.Linear);
			startTime = Time.time;
			OnCircleStarted?.Invoke();
			myCollider.enabled = true;
		}

		private void ResetColors ()
		{
			myMaterial.color = originalColor;
			myMaterial.SetVector(emissionColor, originalColor * originalEmissionIntensity);
		}

		private void GrowEnded ()
		{
			OnCircleCompleted?.Invoke(0);
			OnGrowFinishedEvent.Invoke();
			if (TurnOffWhenEnded)
				Deactivate(false);
		}

		public void OnPointerClick (PointerEventData eventData)
		{
			Vector3 clickPos = eventData.pointerCurrentRaycast.worldPosition;
			Vector3 thisPos = transform.position;
			clickPos.z = thisPos.z;
			float accuracy = Mathf.Clamp(TargetRadius - Vector3.Distance(clickPos, thisPos), 0, TargetRadius) / TargetRadius;
			float speed = (Time.time - startTime) / MaxTime;
			float score = (accuracy * AccuracyWeight + speed * SpeedWeight) / (AccuracyWeight + SpeedWeight);
			OnCircleCompleted?.Invoke(score);
			OnClickedEvent.Invoke();
			Deactivate(true);
		}

		public void Deactivate (bool success)
		{
			myCollider.enabled = false;
			DOTween.Pause(transform);
			Color color = success ? successColor : failColor;
			myMaterial.DOColor(color, 0.25f);
			transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 1, 1).SetRelative(true);
			Sequence sequence1 = DOTween.Sequence();
			sequence1.Append(DOTween.To(() => myMaterial.GetVector(emissionColor).w, (float x) => myMaterial.SetVector(emissionColor, color * x), pulseEmissionIntensity, 0.25f));
			sequence1.Append(DOTween.To(() => myMaterial.GetVector(emissionColor).w, (float x) => myMaterial.SetVector(emissionColor, color * x), originalEmissionIntensity, 0.25f));
			sequence1.OnComplete(() =>
			{
				DOTween.Complete(transform, false);
				ResetColors();
				gameObject.SetActive(false);
			});
			sequence1.Play();
		}
	}
}