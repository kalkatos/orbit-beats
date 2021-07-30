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
		public static Action OnF1;
		public static Action OnEsc;

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
			else if (e.type == EventType.KeyDown)
			{
				if (e.keyCode == KeyCode.F1)
					OnF1?.Invoke();
				if (e.keyCode == KeyCode.Escape)
					OnEsc?.Invoke();
			}
		}
	}
}
#endif