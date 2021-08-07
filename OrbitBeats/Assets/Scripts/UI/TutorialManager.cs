using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkatos.Cycles
{
	public class TutorialManager : MonoBehaviour
	{
		public static bool Finished => PlayerPrefs.GetInt("TutorialFinished", 0) == 1;

		[SerializeField] private int assistedCircles = 3;
		[SerializeField] private float timeForHelper = 5;
		[SerializeField] private GameObject helper;

		private int currentCircle;
		private CircleSpawner circleSpawner;
		private float lastCircleSpawnTime;

		private void Awake ()
		{
			if (Finished)
			{
				gameObject.SetActive(false);
				return;
			}

			helper.SetActive(false);

			CircleView.OnScored += CircleClicked;
			CircleSpawner.OnCircleSpawned += CircleSpawned;

			circleSpawner = FindObjectOfType<CircleSpawner>();
		}

		private void Update ()
		{
			if (!helper.activeSelf && !circleSpawner.SpawnNewCircles && Time.time - lastCircleSpawnTime >= timeForHelper)
			{
				helper.SetActive(true);
			}
		}

		private void OnDestroy ()
		{
			CircleView.OnScored -= CircleClicked;
			CircleSpawner.OnCircleSpawned -= CircleSpawned;
		}

		private void CircleSpawned (CircleView circle)
		{
			circleSpawner.SpawnNewCircles = false;
			lastCircleSpawnTime = Time.time;
		}

		private void CircleClicked (float obj)
		{
			currentCircle++;
			circleSpawner.SpawnNewCircles = true;
			helper.SetActive(false);
			if (currentCircle >= assistedCircles)
				EndTutorial();
		}

		private void EndTutorial ()
		{
			//PlayerPrefs.SetInt("TutorialFinished", 1);
			CircleView.OnScored -= CircleClicked;
			CircleSpawner.OnCircleSpawned -= CircleSpawned;
			circleSpawner.SpawnNewCircles = true;
			gameObject.SetActive(false);
		}
	}
}