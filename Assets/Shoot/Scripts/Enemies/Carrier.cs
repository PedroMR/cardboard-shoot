using UnityEngine;
using System.Collections;

public class Carrier : MonoBehaviour
{
	public float TimeToReload = 5.0f;
	private float timeUntilShot;
	private CityShooter shooter;

	// Use this for initialization
	void Start()
	{
		timeUntilShot = TimeToReload;
		shooter = GetComponentInChildren<CityShooter>();
	}
	
	// Update is called once per frame
	void Update()
	{
		timeUntilShot -= Time.deltaTime;
		if (timeUntilShot <= 0) {
			timeUntilShot = TimeToReload;

			Shoot();
		}
	
	}

	void Shoot()
	{
		var target = GameController.Instance.FindCityTargetClosestTo(transform.position);

		if (target != null) {
			
			shooter.LaunchAgainstTarget(target.weaponTarget);
		}
	}
}

