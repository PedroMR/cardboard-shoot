using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlideControl : MonoBehaviour {
	public GameObject CameraStartMarker;
	public float TouchTimeToStart = 3.0f;

	private Vector3 targetPosition;
	private Quaternion targetRotation;

	private Sequence beginSequence;

	private bool started = false;

	// Use this for initialization
	void Start () {
		ActivateGame(false);

		var cameraObject = GameObject.Find("Camera");

		targetPosition = cameraObject.transform.position;
		targetRotation = cameraObject.transform.rotation;
		cameraObject.transform.position = CameraStartMarker.transform.position;
		cameraObject.transform.rotation = CameraStartMarker.transform.rotation;

		var cameraTrans = cameraObject.transform;

		beginSequence = DOTween.Sequence().Pause()//.SetDelay(3)
			.Append(cameraTrans.DOMove(targetPosition, 20))
			.Insert(7, cameraTrans.DORotateQuaternion(targetRotation, 6))
			.AppendCallback(BeginGame);
	}

	public void BeginIntroSequence() {
		Debug.Log("Begin intro!");
		beginSequence.Play();
		started = true;
	}

	void BeginGame() {
		ActivateGame(true);
		GameController.Instance.BeginGame();
	}

	void EnableComponent<T>(GameObject go, bool enable) where T : Behaviour {
		var component = go.GetComponent<T>();
		if (component != null) {
			component.enabled = enable;
		}
	}

	void ActivateObject(GameObject go, bool activate) {
		if (go != null) go.SetActive(activate);
	}

	float touchingFor = 0;

	void Update () {
		if (!started) {
			if (Input.touchCount > 0 || Input.GetMouseButton(0)) {
				touchingFor += Time.deltaTime;
				
				if (touchingFor > TouchTimeToStart) {
					BeginIntroSequence();
				}
			} else {
				touchingFor = 0;
			}
		}
	}

	void ActivateGame(bool gameActive) {
		var cameraObject = GameObject.Find("Camera");

		EnableComponent<GvrGaze>(cameraObject, gameActive);
		EnableComponent<GvrHead>(cameraObject, gameActive);
		EnableComponent<StereoController>(cameraObject, gameActive);
		EnableComponent<GvrViewer>(GameObject.Find("GvrViewerMain"), gameActive);
		ActivateObject(GameObject.Find("GvrReticle"), gameActive);

	}
}
