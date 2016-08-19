using UnityEngine;
using System.Collections;

public class DestroyForPoints : MonoBehaviour
{
	public int ScoreValue;

	WeaponTargetable weaponTarget;

	// Use this for initialization
	void Start()
	{
		if (weaponTarget == null) {
			weaponTarget = GetComponentInChildren<WeaponTargetable>();
		}

		weaponTarget.SufferedLethalDamage += OnSufferedLethalDamage;
	}

	public void OnSufferedLethalDamage(WeaponTargetable obj)
	{
		GameController.Instance.Score += ScoreValue;
		GameObject.Destroy(gameObject);
	}

}

