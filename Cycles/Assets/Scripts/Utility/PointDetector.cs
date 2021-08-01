using System;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kalkatos.Cycles
{
	[ExecuteInEditMode]
	public class PointDetector : MonoBehaviour
	{
		public static Action<Vector2> OnMouseLeftClick;
		public static Action<Vector2> OnMouseRightClick;

		public Camera canvasCamera;

#if UNITY_EDITOR
		private CircleSpawnMarker selectedMarker;

		private void Awake ()
		{
			CircleSpawnMarkerInspector.OnMarkerSelection += MarkerSelection;
		}

		private void OnDestroy ()
		{
			CircleSpawnMarkerInspector.OnMarkerSelection -= MarkerSelection;
		}

		private void OnGUI ()
		{
			Event e = Event.current;
			switch (e.type)
			{
				case EventType.MouseDown:
					if (e.button == 0)
						OnMouseLeftClick?.Invoke(e.mousePosition);
					else if (e.button == 1)
						OnMouseRightClick?.Invoke(e.mousePosition);
					break;
				case EventType.Repaint:
				case EventType.Layout:
					EditorUtility.SetDirty(this); // this is important, if omitted, "Mouse down" will not be display
					break;
			}

			//if (e.type == EventType.Layout || e.type == EventType.Repaint)
			//{
			//	EditorUtility.SetDirty(this); // this is important, if omitted, "Mouse down" will not be display
			//}
			//else if (e.type == EventType.MouseDown && e.button == 1)
			//{
			//	OnMouseClick?.Invoke(e.mousePosition);
			//}
		}

		private void OnDrawGizmos ()
		{
			if (selectedMarker)
			{
				Handles.color = Color.magenta;
				Vector3 worldPoint = GameVariables.CoordToWorld(selectedMarker.Position);
				Handles.DrawWireDisc(worldPoint, Vector3.back, 0.5f);
			}
		}

		private void MarkerSelection (CircleSpawnMarker marker)
		{
			selectedMarker = marker;
		}
#endif
	}
}