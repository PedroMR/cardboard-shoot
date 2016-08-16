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
		weaponTarget.SufferedLethalDamage += OnSufferedLethalDamage;
	}

	public void OnSufferedLethalDamage(WeaponTargetable which)
	{
		transform.parent.DOMoveY(-15, 5);

		var original = transform.parent.position;
		transform.parent.DOShakePosition(10, (Vector3.right + Vector3.forward) * 0.3f);
//		transform.parent.DOMoveX(original., 5);
	}
}
