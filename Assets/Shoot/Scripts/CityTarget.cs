using UnityEngine;
using DG.Tweening;

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
		weaponTarget.SufferedDamage += OnSufferedDamage;
		weaponTarget.SufferedLethalDamage += OnSufferedLethalDamage;
	}

	public void OnSufferedDamage(WeaponTargetable which)
	{
		transform.parent.DOShakePosition(1, (Vector3.right + Vector3.forward) * 0.1f);
	}

	public void OnSufferedLethalDamage(WeaponTargetable which)
	{

		var original = transform.parent.position;
		transform.parent.DOShakePosition(4, (Vector3.right + Vector3.forward) * 0.3f);
		transform.parent.DOMoveY(-15, 5);
//		transform.parent.DOMoveX(original., 5);
	}
}
