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
	public float EnemySpawnDistance = 60f;
	public float EnemyMinElevation = 15f;
	public float EnemyMaxElevation = 45f;

	public float WAVE_ENEMY_SEPARATION = 10f;

	public int WAVE_MIN_ENEMIES = 2, WAVE_MAX_ENEMIES = 3;

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
			SpawnEnemyWave();
		}
	
		if (Input.GetKeyDown (KeyCode.J)) {
			SpawnEnemyWave();
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

	void SpawnEnemyWave ()
	{
		var waveCenter = Vector3.zero;
		var polar = Random.value * Mathf.PI * 2;
		var elevation = Mathf.Deg2Rad * Random.Range(EnemyMinElevation, EnemyMaxElevation);
		Util.SphericalToCartesian(EnemySpawnDistance, polar, elevation, out waveCenter);

		var enemiesInWave = Random.Range(WAVE_MIN_ENEMIES,WAVE_MAX_ENEMIES+1);
		var src = Enemy;
		for (var i=0; i < enemiesInWave; i++) {
			var obj = GameObject.Instantiate (src);
			hud.TrackObject (obj.GetComponent<Targetable> ());
			
			var delta = Random.onUnitSphere * WAVE_ENEMY_SEPARATION;
			var pos = waveCenter + delta;
			obj.transform.position = pos;
			obj.transform.LookAt(Vector3.zero);
		}

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
		/**/
	}
}
