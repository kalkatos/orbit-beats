using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kalkatos.Cycles
{
	[DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;

		private void Awake ()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}
		}

		public static void LoadLevel (int number)
		{
			SceneManager.LoadScene("Level" + number);
		}
	}
}
