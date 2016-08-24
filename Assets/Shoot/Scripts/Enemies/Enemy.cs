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

	CityTarget myTarget;
	GvrAudioSource audioSource;
	float initialDistance;
	bool fired = false;

	public Enemy()
	{
	}

	public CityTarget FindTarget()
	{
		var target = GameController.Instance.FindCityTargetClosestTo(this.transform.position);

		if (target != null)
			initialDistance = Vector3.Distance(target.transform.position, this.transform.position);

		return target;
	}

	public void Start()
	{
		audioSource = GetComponent<GvrAudioSource>();
		if (audioSource != null)
			audioSource.pitch = 1;

		myTarget = FindTarget();
		var targetPos = Vector3.zero;

		if (myTarget == null) {
			Debug.Log("No target!");
		} else {
			targetPos = myTarget.transform.position + Vector3.up * 9;
		}

		var waypoints = CreatePathToBuilding(targetPos);

		transform.LookAt(targetPos);
		transform.DOPath(waypoints, PATH_SPEED, PathType.CatmullRom, PathMode.Full3D, 10, Color.magenta)
			.SetLookAt(0f)
			.SetSpeedBased()
			.SetEase(Ease.InQuad)
			.SetAutoKill()
			.OnComplete(OnPathComplete)
			.OnWaypointChange(OnWaypointChanged)
			;
		

		Model.transform.DOScale(0.01f, 0.5f).SetDelay(0.5f).SetEase(Ease.InQuad).From();
	}

	private Vector3[] CreatePathToBuilding(Vector3 targetPos)
	{
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

		return waypoints;
	}

	public void OnPathComplete()
	{
//		ObjectPool.instance.PoolObject(gameObject);
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

