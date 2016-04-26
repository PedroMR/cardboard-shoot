using UnityEngine;
using DG.Tweening;
using SWS;

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

		Vector3[] waypoints = new Vector3[3];

		waypoints[0] = this.transform.position;
		waypoints[1] = targetPos;

		var d = waypoints[1] - waypoints[0];
		var n = Vector3.up;
		var delta = (d - 2 * Vector3.Dot(d, n) * n);
		delta.x *= 2.0f;
		delta.z *= 2.0f;
		var r = delta + waypoints[1];
		waypoints[2] = r;

		transform.LookAt(targetPos);
		transform.DOPath(waypoints, 14.0f, PathType.CatmullRom, PathMode.Full3D, 10, Color.magenta)
			.SetLookAt(0f)
			.SetEase(Ease.InQuad)
//			.SetLoops(-1, LoopType.Yoyo)
			.SetAutoKill()
			.OnComplete(OnPathComplete)
			;

//		transform.DOMove(targetPos, 10.0f).SetLoops(3, LoopType.Yoyo).SetAutoKill();
//		transform.DOMove(targetPos, 1.0f).SetSpeedBased().SetEase(Ease.Linear).SetLoops(3, LoopType.Yoyo).SetAutoKill();


	}

	public void OnPathComplete()
	{
		Destroy(gameObject);
	}
}

