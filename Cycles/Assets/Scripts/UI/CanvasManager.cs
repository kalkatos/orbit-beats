using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kalkatos.Cycles
{
    public class CanvasManager : MonoBehaviour
    {
		[SerializeField] private Button nextLevelButton;

		private void Awake ()
		{
			nextLevelButton.onClick.AddListener(OnClickNextLevelButton);
		}

		private void OnDestroy ()
		{
			nextLevelButton.onClick.RemoveAllListeners();
		}

		private void OnClickNextLevelButton ()
		{
			GameManager.LoadLevel(1);

		}
	}
}