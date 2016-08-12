using UnityEngine;
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

	// Use this for initialization
	void Start()
	{
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
		transform.LookAt(targetPos);
		speed = Mathf.Min(speed + Acceleration * Time.deltaTime, MaxSpeed);
		transform.Translate(Vector3.forward * speed * Time.deltaTime);

		var deltaToTarget = targetPos - transform.position;
		if (deltaToTarget.sqrMagnitude < TRIGGER_DISTANCE_SQ) {
			ReachedTarget = true;

			var explosion = GameObject.Instantiate(ExplosionEffect, transform.position, transform.rotation);
			Destroy(this.gameObject);

			if (Target != null) {
				Target.Health -= Damage;
				if (Target.Health <= 0) //FIXME this should be done by the damage sufferer
					Destroy(Target.gameObject);
			}
		}				
	}
}

