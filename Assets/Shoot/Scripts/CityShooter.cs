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

	public Missile LaunchAgainstTarget(Targetable target)
	{
		var obj = (GameObject)GameObject.Instantiate(RocketPrefab, transform.position, Quaternion.identity);

		var missile = obj.GetComponent<Missile>();
		missile.Target = target;

		return missile;
	}
}

