using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	public void StartGame() {
		VRPreferences.Instance.SaveSettings();
		SceneManager.LoadScene("Game");
	}


}