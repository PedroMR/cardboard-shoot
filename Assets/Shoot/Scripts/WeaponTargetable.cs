using UnityEngine;
using System.Collections;

public class WeaponTargetable : MonoBehaviour
{
	public delegate void Callback(WeaponTargetable target);
	public Callback WasDestroyed;
	public Callback SufferedLethalDamage;

	public int Health = 100;
	public bool Dead = false;

	// Use this for initialization
	void Start()
	{
	
	}
	
	void OnDestroy() {
		if (WasDestroyed != null)
			WasDestroyed(this);
	}

	public void SufferDamage(int amount)
	{
		if (Dead)
			return;
		
		Health -= amount;
		if (Health <= 0) {
			Dead = true;
			Health = 0;

			if (SufferedLethalDamage != null)
				SufferedLethalDamage(this);
		}
	}
}

