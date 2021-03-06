﻿using UnityEngine;
using DG.Tweening;

public class CityTarget : MonoBehaviour
{
	public int Health {
		set {
			weaponTarget.Health = value;
		}
		get {
			return weaponTarget.Health;
		}
	}

	public WeaponTargetable weaponTarget;

	public CityTarget()
	{
	}

	void Awake()
	{
		if (weaponTarget == null)
			weaponTarget = GetComponent<WeaponTargetable>();
		weaponTarget.SufferedDamage += OnSufferedDamage;
		weaponTarget.SufferedLethalDamage += OnSufferedLethalDamage;
	}

	public void OnSufferedDamage(WeaponTargetable which)
	{
		transform.parent.DOShakePosition(1, (Vector3.right + Vector3.forward) * 0.1f);
	}

	public void OnSufferedLethalDamage(WeaponTargetable which)
	{

		transform.parent.DOShakePosition(4, (Vector3.right + Vector3.forward) * 0.3f);
		transform.parent.DOMoveY(-15, 5);
	}
}
