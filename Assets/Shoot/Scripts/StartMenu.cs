using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {
	public GameObject uiMenu;

	void Start() {
		Cardboard.SDK.Recenter();

		if (uiMenu != null) {
			var menuCopy = (GameObject)GameObject.Instantiate(uiMenu, uiMenu.transform.position, uiMenu.transform.rotation);

			MirrorPosition(menuCopy);
		}
	}

	void MirrorPosition(GameObject menu) {
		var newPos = menu.transform.position;

		newPos.x = -newPos.x;
		newPos.z = -newPos.z;

		menu.transform.position = newPos;

		menu.transform.Rotate(0, 180, 0);
			
	}

	public void StartGame() {
		VRPreferences.Instance.SaveSettings();
		SceneManager.LoadScene("Game");

	}


}