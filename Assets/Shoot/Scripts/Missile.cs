using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Missile : MonoBehaviour
{
	public Vector3 FlyingToPosition;
	public float MaxSpeed = 20.0f;
	public float Acceleration = 8f;
	private float speed = 0;
	public float TRIGGER_DISTANCE_SQ = 2 * 2;
	public GameObject ExplosionEffect;
	private bool ReachedTarget = false;

	public int Damage = 5;

	private WeaponTargetable _target;
	public WeaponTargetable Target {
		get { return _target; }
		set { 
			if (_target != null)
				_target.WasDestroyed -= OnTargetDestroyed;
			
			_target = value;  

			if (_target != null)
				_target.WasDestroyed += OnTargetDestroyed;
		}
	}

	void OnEnable()
	{
		ReachedTarget = false;
		speed = 0;
	}

	public void OnTargetDestroyed(WeaponTargetable target) 
	{
		FlyingToPosition = Target.transform.position;
		Target = null;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (ReachedTarget)
			return;

		var targetPos = Target ? Target.transform.position : FlyingToPosition;

		if (!followPath) {
			transform.LookAt(targetPos);
			speed = Mathf.Min(speed + Acceleration * Time.deltaTime, MaxSpeed);
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}

		var deltaToTarget = targetPos - transform.position;
		if (deltaToTarget.sqrMagnitude < TRIGGER_DISTANCE_SQ) {
			ReachedTarget = true;

			var explosionName = ExplosionEffect.name;
			var explosion = ObjectPool.instance.GetObjectForType(explosionName, false);
			explosion.transform.position = transform.position;
			explosion.transform.rotation = transform.rotation;
//			var explosion = GameObject.Instantiate(ExplosionEffect, transform.position, transform.rotation);
//			Destroy(this.gameObject);
			ObjectPool.instance.PoolObject(this.gameObject);

			if (Target != null) {
				Target.SufferDamage(Damage);
			}
		}				
	}

	private Vector3 waypoint;
	private bool followPath;
	public void SetWaypoint(Vector3 waypoint)
	{
		this.waypoint = waypoint;
		followPath = true;
		var waypoints = CreatePath(waypoint);

		transform.LookAt(waypoint);
		transform.DOPath(waypoints, MaxSpeed, PathType.CatmullRom, PathMode.Full3D, 10, Color.yellow)
			.SetLookAt(0f)
			.SetSpeedBased()
			.SetEase(Ease.OutQuad)
			.SetAutoKill()
//			.OnComplete(OnPathComplete)
//			.OnWaypointChange(OnWaypointChanged)
			;
		
	}

	public Vector3 GenerateRandomWaypoint()
	{
		var tForCurve = 0.15f;
		var curveRadius = 15.0f;
		var targetPos = Target ? Target.transform.position : FlyingToPosition;
		var d = targetPos - this.transform.position;
		var toT = d * tForCurve;

		//		var angle = UnityEngine.Random.value * Mathf.PI * 2;
		var random = UnityEngine.Random.onUnitSphere;
		var projected = Vector3.ProjectOnPlane(random, d);
		projected.Normalize();
		projected *= curveRadius;
		return projected + transform.position + toT;

	}

	private Vector3[] CreatePath(Vector3 waypoint)
	{
		var targetPos = Target ? Target.transform.position : FlyingToPosition;

		Vector3[] waypoints = new Vector3[3];

		waypoints[0] = this.transform.position;
		waypoints[1] = waypoint;
		waypoints[2] = targetPos;



		return waypoints;
	}
}

