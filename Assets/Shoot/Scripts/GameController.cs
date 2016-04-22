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
	public GameObject City;

	private float timeSinceSpawn;
	public float TimeToSpawnEnemy = 8.0f;
	public float EnemySpawnDistance = 40f;
	public float EnemyMinElevation = 15f;
	public float EnemyMaxElevation = 65f;

	public static GameController Instance;

	public GameController()
	{
		Instance = this;
	}

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

		if (PlayerTurretHead != null) {
			PlayerTurretHead.transform.rotation = MainCamera.transform.rotation;
			PlayerTurretHead.transform.Rotate(0, -90, 0);
		}
	}

	void SpawnEnemy ()
	{
		var src = Enemy;
		var obj = GameObject.Instantiate (src);
		hud.TrackObject (obj.GetComponent<Targetable> ());

		var pos = Vector3.zero;
		var polar = Random.value * Mathf.PI * 2;
		var elevation = Mathf.Deg2Rad * Random.Range(EnemyMinElevation, EnemyMaxElevation);
		Util.SphericalToCartesian(EnemySpawnDistance, polar, elevation, out pos);
		obj.transform.position = pos;
		obj.transform.LookAt(Vector3.zero);
		/*
		var pathId = "att3";
		if (WaypointManager.Paths.ContainsKey(pathId)) {
			var path = WaypointManager.Paths [pathId];
			var splineMove = obj.AddComponent<splineMove> ();
			splineMove.speed = 8;
			splineMove.SetPath (path);
			splineMove.pathMode = DG.Tweening.PathMode.Full3D;
			splineMove.lookAhead = 0.3f;
			splineMove.StartMove ();
		}
		*/
	}
}
