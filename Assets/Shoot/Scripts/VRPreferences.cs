using UnityEngine;
using System.Collections;

public class VRPreferences : MonoBehaviour
{
	public static bool VRMode = false;
	public static Cardboard.DistortionCorrectionMethod DistortionCorrection = Cardboard.DistortionCorrectionMethod.Unity;
	public static bool DirectRender = true;

	public static VRPreferences Instance;

	// Use this for initialization
	void Start()
	{
		Instance = this;
		RestoreSettings();
	}

	public void RestoreSettings()
	{
		Cardboard.SDK.VRModeEnabled = VRMode;
		Cardboard.SDK.DistortionCorrection = DistortionCorrection;
		Cardboard.Controller.directRender = DirectRender;
	}

	public void SaveSettings()
	{
		VRMode = Cardboard.SDK.VRModeEnabled;
		DistortionCorrection = Cardboard.SDK.DistortionCorrection;
		DirectRender = Cardboard.Controller.directRender;
	}
	
	// Update is called once per frame
	void Update()
	{
	
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

	public void ToggleVRMode() {
		Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
	}
}

