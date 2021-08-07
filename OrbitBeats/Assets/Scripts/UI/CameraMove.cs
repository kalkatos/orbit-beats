using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkatos.Cycles
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Vector3 direction;

		private void Awake ()
		{
			direction.Normalize();
		}

		private void OnValidate ()
		{
			direction.Normalize();
		}

		private void Update ()
		{
			transform.Rotate(direction * speed, Space.World);
		}
	}
}
