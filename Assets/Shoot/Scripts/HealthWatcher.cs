using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthWatcher : MonoBehaviour {
	public CityTarget target;
	public Text label;

	// Use this for initialization
	void Start () {
		if (label == null)
			label = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (label != null) {
			label.text = target.Health.ToString();
		}
	}
}
