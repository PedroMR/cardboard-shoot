using UnityEngine;
using System.Collections;

public class HideWhenPlaying : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		gameObject.SetActive(false);
	}
}
