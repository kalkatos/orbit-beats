using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kalkatos.Cycles
{
	public class CircleSpawnMarker : Marker, INotification
	{
		public Vector2 Position;
		public CircleDefinitionNames PrefabName;
		public float MaxTime = 3f;
		public float TargetRadius = 1f;
		public float SpeedWeight = 1f;
		public float AccuracyWeight = 1f;

		public PropertyName id => $"{PrefabName}@{Position.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}";

		public override string ToString ()
		{
			return $"Marker {id} (MaxTime: {MaxTime.ToString("0.0")}, TargetRadius: {TargetRadius.ToString("0.0")}, SpeedWeight: {SpeedWeight.ToString("0.0")}, AccuracyWeight: {AccuracyWeight.ToString("0.0")})";
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(CircleSpawnMarker))]
	public class CircleSpawnMarkerInspector : Editor
	{
		private bool isGettingScreenPoint;
		private CircleSpawnMarker circleViewMarker;
		private Vector2 gameResolution;

		private void OnEnable ()
		{
			circleViewMarker = (CircleSpawnMarker)target;
			gameResolution = new Vector2(1200, 800);
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();
			GUILayout.Space(15);
			gameResolution = EditorGUILayout.Vector2Field("Game resolution", gameResolution);
			GUILayout.Space(5);
			if (!isGettingScreenPoint)
			{
				if (GUILayout.Button("Get Point on Game View"))
				{
					PointDetector.OnMouseClick += MouseClickOnGameView;
					isGettingScreenPoint = true;
				}
			}
			else
				EditorGUILayout.LabelField("Getting point on Game View", EditorStyles.boldLabel);
		}

		private void MouseClickOnGameView (Vector2 point)
		{
			PointDetector.OnMouseClick -= MouseClickOnGameView;
			circleViewMarker.Position.x = point.x / gameResolution.x;
			circleViewMarker.Position.y = (gameResolution.y - point.y) / gameResolution.y;
			isGettingScreenPoint = false;
			Repaint();
			EditorUtility.SetDirty(target); 
		}
	}
#endif
}
