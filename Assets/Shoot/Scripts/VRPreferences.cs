using UnityEngine;
using Gvr;
using System.Collections;

public class VRPreferences : MonoBehaviour
{
	public static bool VRMode = false;

	public static bool DistortionCorrection = true;
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
		GvrViewer.Instance.DistortionCorrectionEnabled = DistortionCorrection;
		GvrViewer.Controller.directRender = DirectRender;
	}

	public void SaveSettings()
	{
		VRMode = GvrViewer.Instance.VRModeEnabled;
		DistortionCorrection = GvrViewer.Instance.DistortionCorrectionEnabled;
		DirectRender = GvrViewer.Controller.directRender;
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	public void ToggleDistortionCorrection() {
		GvrViewer.Instance.DistortionCorrectionEnabled = !GvrViewer.Instance.DistortionCorrectionEnabled;
	}

	public void ToggleDirectRender() {
		GvrViewer.Controller.directRender = !GvrViewer.Controller.directRender;
	}

	public void ToggleVRMode() {
		GvrViewer.Instance.VRModeEnabled = !GvrViewer.Instance.VRModeEnabled;
	}
}

