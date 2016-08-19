using UnityEngine;
using System.Collections;

public class CityShooter : MonoBehaviour
{
	public GameObject RocketPrefab;

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	public Missile LaunchAgainstTarget(WeaponTargetable target)
	{
		return LaunchAgainstTarget(target, Vector3.zero, null);
	}

	public Missile LaunchAgainstTarget(WeaponTargetable target, Vector3 offset, Vector3? waypoint)
	{
		var sourcePosition = transform.position + offset;

		var obj = (GameObject)GameObject.Instantiate(RocketPrefab, sourcePosition, Quaternion.identity);

		var missile = obj.GetComponent<Missile>();
		missile.Target = target;

		if (waypoint.HasValue)
			missile.SetWaypoint(waypoint.GetValueOrDefault());

		return missile;
	}
}

