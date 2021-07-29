using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkatos.Cycles
{
    public class CanvasManager : MonoBehaviour
    {
		private void Awake ()
		{
			for (int i = 0; i < transform.childCount; i++)
				transform.GetChild(i).gameObject.SetActive(true);
		}
	}
}