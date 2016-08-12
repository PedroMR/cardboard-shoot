using UnityEngine;
using System.Collections;

public class WeaponTargetable : MonoBehaviour
{
	public delegate void Callback(WeaponTargetable target);
	public Callback WasDestroyed;

	public int Health = 100;

	// Use this for initialization
	void Start()
	{
	
	}
	
	void OnDestroy() {
		if (WasDestroyed != null)
			WasDestroyed(this);
	}

	// Update is called once per frame
	void Update()
	{
	
	}
}

