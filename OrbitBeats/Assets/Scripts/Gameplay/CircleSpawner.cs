using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Kalkatos.Cycles
{
	public class CircleSpawner : MonoBehaviour, INotificationReceiver
	{
		public static Action<CircleView> OnCircleSpawned;

		public bool SpawnNewCircles = true;

		public void OnNotify (Playable origin, INotification notification, object context)
		{
			if (SpawnNewCircles && notification is CircleSpawnMarker)
			{
				CircleSpawnMarker marker = (CircleSpawnMarker)notification;

				if (Application.isPlaying)
				{

					CircleView circleView = ObjectPool.GetObject(marker.PrefabName.ToString(), 
						GameVariables.CoordToWorld(marker.Position), Quaternion.identity, false).GetComponent<CircleView>();
					if (marker.OverrideDefaults)
					{
						circleView.TimeToActivate = marker.TimeToActivate;
						circleView.TimeActive = marker.TimeActive;
						circleView.Size = marker.Size;
					}
					circleView.gameObject.SetActive(true);
					OnCircleSpawned?.Invoke(circleView);
				}
				else
					Debug.Log(marker);
			}
		}
	}
}
