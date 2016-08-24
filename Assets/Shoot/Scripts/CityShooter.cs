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
		if (RocketPrefab == null) {
			Debug.LogError("No prefab assigned to this shooter! "+ gameObject.name);
			return null;
		}

		var sourcePosition = transform.position + offset;

		var obj = CreateRocketAt(RocketPrefab, sourcePosition);

		var missile = obj.GetComponent<Missile>();
		missile.Target = target;

		if (waypoint.HasValue)
			missile.SetWaypoint(waypoint.GetValueOrDefault());

		return missile;
	}

	private GameObject CreateRocketAt(GameObject prefab, Vector3 sourcePosition) {
		GameObject obj;

		obj = ObjectPool.instance.GetObjectForType(prefab.name, false);

		if (obj == null) {
//			Debug.Log("Did not find missile in pool");
			obj = (GameObject)GameObject.Instantiate(RocketPrefab);
		}

		obj.transform.position = sourcePosition;

		return obj;
	}

}

