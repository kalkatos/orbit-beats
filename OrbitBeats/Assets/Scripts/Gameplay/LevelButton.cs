using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Kalkatos.Cycles
{
    public class LevelButton : MonoBehaviour
	{
		[SerializeField] private Button button;
		[SerializeField] private TextMeshProUGUI levelText;
		[SerializeField] private int levelNumber;

		private void Awake ()
		{
			int lastLevel = PlayerPrefs.GetInt("LastLevel", 0);
			SetLockedState(lastLevel < levelNumber);
		}

		public void SetLockedState (bool locked)
		{
			bool comingSoon = levelNumber == -1;
			button.interactable = !(locked || comingSoon);
			if (comingSoon)
				levelText.text = "Coming Soon";
			else
				levelText.text = locked ? "Locked" : "";
		}

		public void OnClick ()
		{
			GameManager.LoadLevel(levelNumber);
		}
	}
}
