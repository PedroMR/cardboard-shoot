using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	void Start() {
		Cardboard.SDK.Recenter();
	}

	public void StartGame() {
		VRPreferences.Instance.SaveSettings();
		SceneManager.LoadScene("Game");

	}


}