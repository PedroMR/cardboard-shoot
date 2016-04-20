using UnityEngine;
using System.Collections;
using DG.Tweening.Core;
using SWS;

public class GameController : MonoBehaviour {
	public TargetingHUD hud;
	public GameObject Enemy;
	public GameObject RocketPrefab;
	public GameObject PlayerTurretSpawn;
	public GameObject MainCamera;
	public GameObject PlayerTurretHead;

	private float timeSinceSpawn;
	public float TimeToSpawnEnemy = 8.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		timeSinceSpawn += Time.deltaTime;
		if (timeSinceSpawn > TimeToSpawnEnemy) {
			timeSinceSpawn = 0;
			SpawnEnemy();
		}
	
		if (Input.GetKeyDown (KeyCode.J)) {
			SpawnEnemy();
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			var src = RocketPrefab;
			var spawner = PlayerTurretSpawn.transform;
			var spawnPoint = spawner.position;
			var obj = (GameObject)GameObject.Instantiate(src, spawnPoint, spawner.rotation);

			var rigidBody = obj.GetComponent<Rigidbody> ();
			rigidBody.AddRelativeForce(2000, 0, 0);
//			hud.TrackObject(obj.GetComponent<Targetable>());
		}

		if (Input.GetKey (KeyCode.F))
			Time.timeScale = 16;
		else
			Time.timeScale = 1;

		PlayerTurretHead.transform.rotation = MainCamera.transform.rotation;
		PlayerTurretHead.transform.Rotate (0, -90, 0);
	}

	void SpawnEnemy ()
	{
		var src = Enemy;
		var obj = GameObject.Instantiate (src);
		hud.TrackObject (obj.GetComponent<Targetable> ());
		var path = WaypointManager.Paths ["att3"];
		var splineMove = obj.AddComponent<splineMove> ();
		splineMove.speed = 8;
		splineMove.SetPath (path);
		splineMove.pathMode = DG.Tweening.PathMode.Full3D;
		splineMove.lookAhead = 0.3f;
		splineMove.StartMove ();
	}
}
