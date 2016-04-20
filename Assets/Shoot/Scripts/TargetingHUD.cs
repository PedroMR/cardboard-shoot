using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetingHUD : MonoBehaviour {
	public GameObject TargetingUI;
	public float DistanceToElements = 0.5f;

	ICollection<Targetable> targets = new HashSet<Targetable>();
	Dictionary<Targetable, GameObject> targetUIs = new Dictionary<Targetable, GameObject>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		foreach (var target in targets) {
			UpdatePosition(target);
		}
	
	}

	void UpdatePosition (Targetable target)
	{
		var center = transform.position; 
		var ui = targetUIs [target];

		var delta = target.transform.position - center;
		delta.Normalize();
		delta *= DistanceToElements;

		ui.transform.position = center + delta;
		ui.transform.LookAt (center+target.transform.position);

		var lockProgressUI = ui.gameObject.GetComponent<LockProgressUI> ();
		if (lockProgressUI != null)
		{
			lockProgressUI.SetLockProgress (target.lockProgress);
		}

		if (target.lockProgress >= 1.0f)
		{
			Destroy (target.gameObject, 0.5f);
			Destroy (ui.gameObject, 0.5f);
			targets.Remove (target);
		}
	}

	public void TrackObject(Targetable target) {
		if (targets.Contains (target)) {
			Debug.LogWarning("Already tracking target "+target);
			return;
		}

		targets.Add (target);

		var obj = GameObject.Instantiate(TargetingUI);

		targetUIs.Add (target, obj);

		UpdatePosition (target);
	}
}
