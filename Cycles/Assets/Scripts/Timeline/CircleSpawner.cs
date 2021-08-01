using UnityEngine;
using UnityEngine.Playables;

namespace Kalkatos.Cycles
{
	public class CircleSpawner : MonoBehaviour, INotificationReceiver
	{
		[SerializeField] private Canvas canvas;
		[SerializeField] private float startingX = -100f;
		[SerializeField] private float startingY = 0;
		[SerializeField] private float startingZ = 100;
		[SerializeField] private float minX = -6.5f;
		[SerializeField] private float maxX = 6.5f;
		[SerializeField] private float minY = -4f;
		[SerializeField] private float maxY = 4f;

		public void OnNotify (Playable origin, INotification notification, object context)
		{
			if (notification is CircleSpawnMarker)
			{
				CircleSpawnMarker marker = (CircleSpawnMarker)notification;

				if (Application.isPlaying)
				{
					CircleView circleView = ObjectPool.GetObject(marker.PrefabName.ToString(), new Vector3(
						startingX + minX + (maxX - minX) * marker.Position.x,
						startingY + minY + (maxY - minY) * marker.Position.y,
						startingZ), Quaternion.identity, false).GetComponent<CircleView>();
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
