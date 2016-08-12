using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	public void ToggleVRMode() {
		Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
	}

	public void StartGame() {
		SceneManager.LoadScene("Game");
	}


	public void ToggleDistortionCorrection() {
		switch(Cardboard.SDK.DistortionCorrection) {
			case Cardboard.DistortionCorrectionMethod.Unity:
				Cardboard.SDK.DistortionCorrection = Cardboard.DistortionCorrectionMethod.Native;
				break;
			case Cardboard.DistortionCorrectionMethod.Native:
				Cardboard.SDK.DistortionCorrection = Cardboard.DistortionCorrectionMethod.None;
				break;
			case Cardboard.DistortionCorrectionMethod.None:
			default:
				Cardboard.SDK.DistortionCorrection = Cardboard.DistortionCorrectionMethod.Unity;
				break;
		}
	}

	public void ToggleDirectRender() {
		Cardboard.Controller.directRender = !Cardboard.Controller.directRender;
	}
}