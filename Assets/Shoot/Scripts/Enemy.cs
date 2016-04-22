using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
	CityTarget myTarget;

	public Enemy()
	{
	}

	public CityTarget FindTarget()
	{
		var targets = GameController.Instance.City.GetComponentsInChildren<CityTarget>();

		CityTarget closest = null;
		var closestRange = float.MaxValue;
		foreach (var target in targets) {
			if (target.Health <= 0)
				continue;

			var range = Vector3.Distance(target.transform.position, this.transform.position);
			if (range < closestRange)
			{
				closest = target;
				closestRange = range;
			}
		}

		return closest;
	}

	public void Start()
	{
		myTarget = FindTarget();
		var targetPos = Vector3.zero;

		if (myTarget == null) {
			Debug.Log("No target!");
		} else {
			targetPos = myTarget.transform.position + Vector3.up * 9;
		}

		transform.LookAt(targetPos);
		transform.DOMove(targetPos, 10.0f).SetLoops(3, LoopType.Yoyo).SetAutoKill();
//		transform.DOMove(targetPos, 1.0f).SetSpeedBased().SetEase(Ease.Linear).SetLoops(3, LoopType.Yoyo).SetAutoKill();
	}
}

