using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider))]
public class Targetable : MonoBehaviour {
	bool gazedAt;
	public float RotationSpeed = 0.5f;
	float angle = 0f;

	public float lockProgress = 0f;
	const float LOCK_PER_SECOND = 0.25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gazedAt) {
			lockProgress += LOCK_PER_SECOND * Time.deltaTime;
			lockProgress = Mathf.Clamp01(lockProgress);
		}



		if (false){
			var oldPos = transform.position;
			oldPos.y = 0;
			var range = (oldPos  - Vector3.zero).magnitude;
			angle += RotationSpeed * Time.deltaTime;
			var newPos = new Vector3 ();
			newPos.y = transform.position.y;
			newPos.x = Mathf.Cos (angle) * range;
			newPos.z = Mathf.Sin (angle) * range;
			transform.position = newPos;
			transform.LookAt (Vector3.Cross (newPos, Vector3.up) + newPos);
//			transform.Rotate(Vector3.up, Time.deltaTime*RotationSpeed);
		}
	}

	public void SetGazedAt(bool gazedAt) {
		this.gazedAt = gazedAt;
	}
}
