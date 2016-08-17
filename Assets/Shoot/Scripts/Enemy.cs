using UnityEngine;
using DG.Tweening;
using SWS;

public class Enemy : MonoBehaviour
{
//	static float PATH_DURATION = 22.0f;
	static float PATH_SPEED = 3f;
	public float AudioPitchAtStart = 1.0f;
	public float AudioPitchAtTarget = 3.0f;
	public float AudioPitchAfterTarget = 0.8f;

	public GameObject Model;

	public int scoreValue = 5;

	CityTarget myTarget;
	CardboardAudioSource audioSource;
	float initialDistance;
	bool fired = false;

	public Enemy()
	{
	}

	public CityTarget FindTarget()
	{
		var targets = GameController.Instance.City.GetComponentsInChildren<CityTarget>();

		CityTarget closest = null;
		var closestRange = float.MaxValue;
		foreach (var target in targets) {
			if (target.Health <= 0)
				continue;

			var range = Vector3.Distance(target.transform.position, this.transform.position);
			if (range < closestRange)
			{
				closest = target;
				closestRange = range;
			}
		}

		initialDistance = closestRange;
		return closest;
	}

	public void Start()
	{
		audioSource = GetComponent<CardboardAudioSource>();
		if (audioSource != null)
			audioSource.pitch = 1;

		myTarget = FindTarget();
		var targetPos = Vector3.zero;

		if (myTarget == null) {
			Debug.Log("No target!");
		} else {
			targetPos = myTarget.transform.position + Vector3.up * 9;
		}

		Vector3[] waypoints = new Vector3[3];

		waypoints[0] = this.transform.position;

		var d = targetPos - waypoints[0];
		var projectedDelta = d.normalized;
		projectedDelta.y = 0;

		waypoints[1] = targetPos - projectedDelta * 12.0f;

		var n = Vector3.up;
		var delta = (d - 2 * Vector3.Dot(d, n) * n);
		delta.x *= 2.0f;
		delta.z *= 2.0f;
		var r = delta + waypoints[1];
		waypoints[2] = r;

		transform.LookAt(targetPos);
		transform.DOPath(waypoints, PATH_SPEED, PathType.CatmullRom, PathMode.Full3D, 10, Color.magenta)
			.SetLookAt(0f)
			.SetSpeedBased()
			.SetEase(Ease.InQuad)
//			.SetLoops(-1, LoopType.Yoyo)
			.SetAutoKill()
			.OnComplete(OnPathComplete)
			.OnWaypointChange(OnWaypointChanged)
			;

//		transform.DOMove(targetPos, 10.0f).SetLoops(3, LoopType.Yoyo).SetAutoKill();
//		transform.DOMove(targetPos, 1.0f).SetSpeedBased().SetEase(Ease.Linear).SetLoops(3, LoopType.Yoyo).SetAutoKill();

		var weaponTargetable = GetComponent<WeaponTargetable>();
		if (weaponTargetable != null) {
			weaponTargetable.SufferedLethalDamage += OnSufferedLethalDamage;
		}

		Model.transform.DOScale(0.01f, 0.5f).SetDelay(0.5f).SetEase(Ease.InQuad).From();
	}

	public void OnPathComplete()
	{
		Destroy(gameObject);
	}

	public void OnWaypointChanged(int waypoint)
	{
		if (waypoint == 1) {
			LaunchAttackAgainstTarget();
			if (audioSource != null)
				audioSource.pitch = AudioPitchAfterTarget;
		}
	}

	public void OnSufferedLethalDamage(WeaponTargetable obj)
	{
		GameController.Instance.Score += scoreValue;
		GameObject.Destroy(gameObject);
	}

	void LaunchAttackAgainstTarget()
	{
		fired = true;
		var shooter = GetComponentInChildren<CityShooter>();
		if (shooter != null) {
			if (myTarget.weaponTarget != null) {
				shooter.LaunchAgainstTarget(myTarget.weaponTarget); 
			} else {
				var target = myTarget.GetComponentInChildren<WeaponTargetable>();
				shooter.LaunchAgainstTarget(target); 
			}
		}
	}

	public void Update()
	{
		if (audioSource != null && !fired && myTarget != null) {
			var distance = Vector3.Distance(myTarget.transform.position, this.transform.position);
			var pitch = Mathf.Lerp(AudioPitchAtTarget, AudioPitchAtStart, distance / initialDistance);
			audioSource.pitch = pitch;
		}
	}

}

