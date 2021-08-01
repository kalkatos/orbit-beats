using UnityEngine;
using UnityEngine.Playables;

namespace Kalkatos.Cycles
{
	public class CircleSpawner : MonoBehaviour, INotificationReceiver
	{
		public void OnNotify (Playable origin, INotification notification, object context)
		{
			if (notification is CircleSpawnMarker)
			{
				CircleSpawnMarker marker = (CircleSpawnMarker)notification;

				if (Application.isPlaying)
				{

					CircleView circleView = ObjectPool.GetObject(marker.PrefabName.ToString(), 
						GameVariables.CoordToWorld(marker.Position), Quaternion.identity, false).GetComponent<CircleView>();
					circleView.TimeToActivate = marker.TimeToActivate;
					circleView.TimeActive = marker.TimeActive;
					circleView.Size = marker.Size;
					circleView.gameObject.SetActive(true);
				}
				else
					Debug.Log(marker);
			}
		}
	}
}
