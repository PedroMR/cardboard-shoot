using UnityEngine;
using Gvr;
using System.Collections;

public class VRPreferences : MonoBehaviour
{
	public static bool VRMode = false;

	public static GvrViewer.DistortionCorrectionMethod DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Unity;
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
		GvrViewer.Instance.VRModeEnabled = VRMode;
		GvrViewer.Instance.DistortionCorrection = DistortionCorrection;
		GvrViewer.Controller.directRender = DirectRender;
	}

	public void SaveSettings()
	{
		VRMode = GvrViewer.Instance.VRModeEnabled;
		DistortionCorrection = GvrViewer.Instance.DistortionCorrection;
		DirectRender = GvrViewer.Controller.directRender;
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	public void ToggleDistortionCorrection() {
		switch(GvrViewer.Instance.DistortionCorrection) {
			case GvrViewer.DistortionCorrectionMethod.Unity:
				GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Native;
				break;
			case GvrViewer.DistortionCorrectionMethod.Native:
				GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.None;
				break;
			case GvrViewer.DistortionCorrectionMethod.None:
			default:
				GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Unity;
				break;
		}
	}

	public void ToggleDirectRender() {
		GvrViewer.Controller.directRender = !GvrViewer.Controller.directRender;
	}

	public void ToggleVRMode() {
		GvrViewer.Instance.VRModeEnabled = !GvrViewer.Instance.VRModeEnabled;
	}
}

