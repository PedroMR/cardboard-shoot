using UnityEngine;
using System.Collections;

public class Carrier : MonoBehaviour
{
	public float TimeToReload = 5.0f;
	private float timeUntilShot;
	private CityShooter shooter;
	public int ShotBurst = 3;
	public float ShotPause = 0.5f;

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

			StartCoroutine(Shoot());
		}
	
	}

	IEnumerator Shoot()
	{
		var target = GameController.Instance.FindCityTargetClosestTo(transform.position);

		if (target != null) {
			for (var i = 0; i < ShotBurst; i++) {
				var drone = shooter.LaunchAgainstTarget(target.weaponTarget);
				drone.SetWaypoint(drone.GenerateRandomWaypoint());
				yield return new WaitForSeconds(ShotPause);
			}
		}
	}
}

