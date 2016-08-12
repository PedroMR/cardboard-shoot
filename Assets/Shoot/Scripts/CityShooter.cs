using UnityEngine;
using System.Collections;

public class CityShooter : MonoBehaviour
{
	public GameObject RocketPrefab;
	CardboardAudioSource launchSound;

	// Use this for initialization
	void Start()
	{
		launchSound = GetComponent<CardboardAudioSource>();
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	public Missile LaunchAgainstTarget(Targetable target)
	{
		var obj = (GameObject)GameObject.Instantiate(RocketPrefab, transform.position, Quaternion.identity);

		if (launchSound != null) {
			launchSound.Play();
		}

		var missile = obj.GetComponent<Missile>();
		missile.Target = target;
		return missile;
	}
}

