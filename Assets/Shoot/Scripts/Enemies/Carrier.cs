using UnityEngine;
using System.Collections;

public class Carrier : MonoBehaviour
{
	public float TimeToReload = 5.0f;
	private float timeUntilShot;

	// Use this for initialization
	void Start()
	{
		timeUntilShot = TimeToReload;
	
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
		
	}
}

