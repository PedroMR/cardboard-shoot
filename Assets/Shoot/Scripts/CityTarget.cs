using UnityEngine;

[RequireComponent(typeof(WeaponTargetable))]
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

	private WeaponTargetable weaponTarget;

	public CityTarget()
	{
	}

	void Awake()
	{
		weaponTarget = GetComponent<WeaponTargetable>();
	}
}
