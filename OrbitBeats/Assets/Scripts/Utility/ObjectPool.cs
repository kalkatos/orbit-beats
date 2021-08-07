using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kalkatos.Cycles
{
	public class ObjectPool : MonoBehaviour
	{
		private static ObjectPool instance = null;

		[System.Serializable]
		public class ObjectDefinition
		{
			public CircleDefinitionNames id;
			public GameObject go;
			public int qty;
			public bool isCanvasObject;
			[HideInInspector] public int instantiations;
		}

		[SerializeField] private RectTransform canvasObjectPool;
		[SerializeField] private ObjectDefinition[] objects;

		private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();
		private Dictionary<string, ObjectDefinition> prefabs = new Dictionary<string, ObjectDefinition>();

		private void Awake ()
		{
			if (instance == null)
				instance = this;
			else if (instance != this)
			{
				Destroy(gameObject);
				return;
			}

			for (int i = 0; i < objects.Length; i++)
			{
				List<GameObject> list = new List<GameObject>();
				pool.Add(objects[i].id.ToString(), list);
				prefabs.Add(objects[i].id.ToString(), objects[i]);
				objects[i].go.SetActive(false);
				for (int j = 0; j < objects[i].qty; j++)
				{
					GameObject newObj = Instantiate(objects[i].go, transform.position, Quaternion.identity, objects[i].isCanvasObject ? canvasObjectPool : transform);
					newObj.name = objects[i].id.ToString();
					if (!newObj.GetComponent<PoolObjectDeactivationCaller>())
						newObj.AddComponent<PoolObjectDeactivationCaller>();
					newObj.SetActive(false);
					list.Add(newObj);
				}
			}
		}

		private void OnDestroy ()
		{
			StopAllCoroutines();
			int counter = 0;
			string report = "[ObjectPool] Instantiations: ";
			foreach (var item in prefabs)
			{
				if (counter > 0)
					report += ", ";
				report += $"{item.Value.id} = {item.Value.instantiations}";
				counter++;
			}
			Debug.Log(report);
		}

		private IEnumerator SetParentAfterMiliseconds (GameObject go)
		{
			yield return new WaitForSeconds(0.1f);
			if (prefabs[go.name].isCanvasObject)
				go.transform.SetParent(canvasObjectPool);
			else
				go.transform.SetParent(transform);
		}

		private static void PoolObject (GameObject go)
		{
			go.SetActive(false);
			if (!instance || !instance.gameObject.activeSelf)
				return;

			instance.StartCoroutine(instance.SetParentAfterMiliseconds(go));

			if (instance.pool.ContainsKey(go.name) && !instance.pool[go.name].Contains(go))
				instance.pool[go.name].Add(go);
			else
			{
				List<GameObject> newList = new List<GameObject>();
				newList.Add(go);
				instance.pool.Add(go.name, newList);
			}
		}

		/// <summary>
		/// Gets the object activated.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="name">Name.</param>
		/// <param name="pos">Position.</param>
		public static GameObject GetObject (string name, Vector3 position, Quaternion? rotation = null)
		{
			if (!rotation.HasValue)
				rotation = Quaternion.identity;
			return GetObject(name, position, rotation.Value, true);
		}

		/// <summary>
		/// Gets the object.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="name">Name.</param>
		/// <param name="position">Position.</param>
		/// <param name="activate">If set to <c>true</c> activate.</param>
		public static GameObject GetObject (string name, Vector3 position, Quaternion rotation, bool activate)
		{
			GameObject poolObj = null;

			if (instance.pool.ContainsKey(name))
			{
				List<GameObject> list = instance.pool[name];
				if (list.Count > 0)
				{
					poolObj = list[0];
					list.RemoveAt(0);
					poolObj.transform.position = position;
					poolObj.transform.rotation = rotation;
				}
				else
				{
					instance.prefabs[name].go.SetActive(false);
					poolObj = Instantiate(instance.prefabs[name].go, position, rotation);
					poolObj.name = name;
					instance.prefabs[name].instantiations++;
					if (!poolObj.TryGetComponent(out PoolObjectDeactivationCaller caller))
						poolObj.AddComponent<PoolObjectDeactivationCaller>();
				}
				if (activate)
					poolObj.SetActive(true);
				//caller.Activate();
			}
			else
			{
				if (string.IsNullOrEmpty(name))
					name = "<empty>";
				Debug.LogError("There is not an Object of name " + name + " in pool.");
			}

			return poolObj;
		}

		private class PoolObjectDeactivationCaller : MonoBehaviour
		{
			//private bool activated = false;

			private void OnDisable ()
			{
				//if (activated)
				//{
				PoolObject(gameObject);
				//	activated = false;
				//}
			}

			//public void Activate ()
			//{
			//	activated = true;
			//}
		}
	}
}