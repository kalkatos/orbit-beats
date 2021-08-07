using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kalkatos.Cycles
{
	public class CircleSpawnMarker : Marker, INotification
	{
		public Vector2 Position;
		public CircleDefinitionNames PrefabName;
		public bool OverrideDefaults = false;
		public float TimeToActivate = 1f;
		public float TimeActive = 0.3f;
		public float Size = 1f;

		public PropertyName id => $"{PrefabName}@{Position.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}";

		public override string ToString ()
		{
			return $"Marker {id} (TimeToActivate: {TimeToActivate.ToString("0.0")}, TimeActive: {TimeActive.ToString("0.0")}, Size: {Size.ToString("0.0")})";
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(CircleSpawnMarker))]
	public class CircleSpawnMarkerInspector : Editor
	{
		public static Action<CircleSpawnMarker> OnMarkerSelection; 

		private static bool isGettingScreenPoint;
		private static CircleSpawnMarker circleViewMarker;
		
		private void OnEnable ()
		{
			PointDetector.OnMouseLeftClick += MouseClickOnGameView;
			circleViewMarker = (CircleSpawnMarker)target;
			OnMarkerSelection?.Invoke(circleViewMarker);
		}

		private void OnDisable ()
		{
			PointDetector.OnMouseLeftClick -= MouseClickOnGameView;
			isGettingScreenPoint = false;
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();
			GUILayout.Space(15);
			Event e = Event.current;
			if (!isGettingScreenPoint)
			{
				if (GUILayout.Button("Get Point on Game View") || (e.type == EventType.KeyDown && e.keyCode == KeyCode.F1))
					StartGettingClicks();
			}
			else
				EditorGUILayout.LabelField("Getting point on Game View", EditorStyles.boldLabel);
		}

		private void MouseClickOnGameView (Vector2 point)
		{
			if (isGettingScreenPoint)
			{
				StopGettingClicks();
				circleViewMarker.Position = GameVariables.GetScreenPercent(point);
				Repaint();
				EditorUtility.SetDirty(target);
			}
		}

		[MenuItem("My Commands/Start Getting Clicks _g")]
		private static void StartGettingClicks ()
		{
			isGettingScreenPoint = true;
			Debug.Log("Started getting point from Game Screen");
		}

		[MenuItem("My Commands/Stop Getting Clicks _b")]
		private static void StopGettingClicks ()
		{
			if (isGettingScreenPoint)
				Debug.Log("Stopped getting point from Game Screen");
			isGettingScreenPoint = false;
		}

		[MenuItem("My Commands/Copy Position To Clipboard _c")]
		private static void CopyPositionToClipboard ()
		{
			string position = circleViewMarker.Position.x.ToString("0.0000") + "|";
			position += circleViewMarker.Position.y.ToString("0.0000");
			GUIUtility.systemCopyBuffer = position;
			Debug.Log($"Position {position} copied to clipboard");
		}

		[MenuItem("My Commands/Paste Position From Clipboard _v")]
		private static void PastePositionFromClipboard ()
		{
			string[] positions = GUIUtility.systemCopyBuffer.Split('|');
			circleViewMarker.Position = new Vector2(float.Parse(positions[0]), float.Parse(positions[1]));
			Debug.Log($"Position {GUIUtility.systemCopyBuffer} pasted from clipboard");
		}
	}
#endif
}
