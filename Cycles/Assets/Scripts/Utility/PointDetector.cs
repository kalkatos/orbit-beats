#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

namespace Kalkatos.Cycles
{
	[ExecuteInEditMode]
	public class PointDetector : MonoBehaviour
	{
		public static Action<Vector2> OnMouseClick;

		void OnGUI ()
		{
			Event e = Event.current;
			if (e.type == EventType.Layout || e.type == EventType.Repaint)
			{
				EditorUtility.SetDirty(this); // this is important, if omitted, "Mouse down" will not be display
			}
			else if (e.type == EventType.MouseDown)
			{
				OnMouseClick?.Invoke(e.mousePosition);
			}
		}
	}
}
#endif