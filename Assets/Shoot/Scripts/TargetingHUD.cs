using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetingHUD : MonoBehaviour {
	public GameObject TargetingUI;
	public float DistanceToElements = 2f;

	ICollection<PlayerTargetable> targets = new HashSet<PlayerTargetable>();
	Dictionary<PlayerTargetable, GameObject> targetUIs = new Dictionary<PlayerTargetable, GameObject>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		foreach (var target in targets) {
			UpdatePosition(target);
		}
	}

	void UpdatePosition (PlayerTargetable target)
	{
		var center = transform.position; 
		var ui = targetUIs [target];

		var delta = target.transform.position - center;
		var distance = delta.magnitude;
		delta.Normalize();
		delta *= DistanceToElements;
//		ui.transform.position = center + delta;
		ui.transform.position = target.transform.position - delta;
		ui.transform.localScale = Vector3.one * (distance * 0.5f / 62.32f);

		ui.transform.LookAt (2*(center+target.transform.position));

		var lockProgressUI = ui.gameObject.GetComponent<LockProgressUI> ();
		if (lockProgressUI != null)
		{
			lockProgressUI.SetLockProgress (target.lockProgress);
			lockProgressUI.SetRange(target.transform.position.magnitude * 10);
		}

		if (target.lockProgress >= 1.0f)
		{
			ui.gameObject.SetActive(false);//TODO special blinking effect, remove collider
			var collider = target.GetComponent<Collider>();
			if (collider != null) {
				collider.enabled = false;
			}
			target.SetGazedAt(false);
			target.gameObject.layer = 0;
		}
	}

	public void OnLockProgress(PlayerTargetable target, float currentLock, float prevLock) 
	{
		float beepsPerLock = Mathf.Max(1.9f, 4 * target.SecondsToLock);

		if (prevLock < 1.0f) {
			if (((int)(prevLock * beepsPerLock)) != (int)(currentLock*beepsPerLock) || prevLock == 0) {
				var ui = targetUIs [target];
				if (ui != null) {
					var audio = ui.GetComponent<GvrAudioSource>();
					if (audio != null)
						audio.Play();
				}
			}
		}

	}

	public void TrackObject(PlayerTargetable target) {
		if (targets.Contains (target)) {
			Debug.LogWarning("Already tracking target "+target);
			return;
		}

		targets.Add (target);
		target.OnLockProgress += OnLockProgress;

		var weaponTarget = target.GetComponent<WeaponTargetable>();
		weaponTarget.WasDestroyed += TargetDestroyed;

		var obj = GameObject.Instantiate(TargetingUI);

		targetUIs.Add (target, obj);

		UpdatePosition (target);
	}

	public void TargetDestroyed(WeaponTargetable weaponTarget) {
		var target = weaponTarget.GetComponent<PlayerTargetable>(); 
		var ui = targetUIs [target];
		Destroy(ui.gameObject);
		targets.Remove(target);
		targetUIs.Remove(target);
	}
}
