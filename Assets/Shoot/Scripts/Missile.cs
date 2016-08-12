using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
	public Vector3 FlyingToPosition;
	public float MaxSpeed = 20.0f;
	public float Acceleration = 8f;
	private float speed = 0;
	public float TRIGGER_DISTANCE_SQ = 2 * 2;

	private PlayerTargetable _target;
	public PlayerTargetable Target {
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

	public void OnTargetDestroyed(PlayerTargetable target) 
	{
		FlyingToPosition = Target.transform.position;
		Target = null;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Target == null && FlyingToPosition == null) {
//			FlyingToPosition = transform.position + transform.rotation * 10f;		
		}

		var targetPos = Target ? Target.transform.position : FlyingToPosition;
		if (targetPos != null) {
			transform.LookAt(targetPos);
			speed = Mathf.Min(speed + Acceleration * Time.deltaTime, MaxSpeed);
			transform.Translate(Vector3.forward * speed * Time.deltaTime);

			var deltaToTarget = targetPos - transform.position;
			if (deltaToTarget.sqrMagnitude < TRIGGER_DISTANCE_SQ) {
				Destroy(this.gameObject);
				if (Target != null) {
					Destroy(Target.gameObject); //TODO damage amounts
				}
			}
				
		}
	}
}

