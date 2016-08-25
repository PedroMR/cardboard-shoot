﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using SWS;

public class GameController : MonoBehaviour {
	public TargetingHUD hud;
	public GameObject EnemyCarrier;
	public GameObject Enemy;
	public GameObject RocketPrefab;
	public GameObject PlayerTurretSpawn;
	public GameObject MainCamera;
	public GameObject PlayerTurretHead;
	public GameObject City;
	public GameObject GameOverInfo;

	private float timeUntilSpawn;
	public FloatRange TimeToSpawnEnemy = new FloatRange(6.0f, 9.0f);
	public float TimeToSpawnEnemyAccel = 0.2f;
	public float TimeToSpawnEnemyAccelInterval = 6.00f;
	private float timeUntilEnemySpawnAccel;

	public FloatRange EnemySpawnDistance = new FloatRange(50f, 80f);
	public FloatRange EnemySpawnDelay = new FloatRange(0f, 1f);
	public float EnemySpawnAngleSpread = Mathf.PI / 2;
	public float EnemyMinElevation = 15f;
	public float EnemyMaxElevation = 45f;
	public float EnemyEmptyChance = 0.1f;

	private float CurrentCarrierChance = -1.0f;
	public float CarrierChanceIncreasePerWave = 0f;
	public float CarrierChanceCostToSpawn = 1.5f;

	public float WAVE_ENEMY_SEPARATION = 10f;

	public int WAVE_MIN_ENEMIES = 2, WAVE_MAX_ENEMIES = 3;

	private int numTargetsAlive;

	public delegate void ScoreChange(int newScore);
	public ScoreChange OnScoreChange;

	private int _score;
	public int Score {
		get { return _score; }
		set { 
			_score = value; 
			if (OnScoreChange != null)
				OnScoreChange(_score); 
		}
	}

	enum GameState {
		INTRO,
		PLAYING,
		OVER
	};

	GameState gameState;

	private bool TEST_PERFORMANCE = false;


	public static GameController Instance;

	public GameController()
	{
		Instance = this;
	}

	void Awake() {
		GvrViewer.Instance.Recenter();

	}
	// Use this for initialization
	void Start () {

		var config = Config.Instance;
		var wave = config.GetWaveById(2);
		Debug.Log(config.SelectEnemyGroupForWave(wave).ID);
		Debug.Log(config.SelectEnemyGroupForWave(wave).ID);
		Debug.Log(config.SelectEnemyGroupForWave(wave).ID);
		Debug.Log(config.SelectEnemyGroupForWave(wave).ID);
		Debug.Log(config.SelectEnemyGroupForWave(wave).ID);
		Debug.Log(config.SelectEnemyGroupForWave(wave).ID);
		Debug.Log(config.SelectEnemyGroupForWave(wave).ID);

		Score = 0;
		GameOverInfo.SetActive(false);
		gameState = GameState.PLAYING;

		var targets = City.GetComponentsInChildren<CityTarget>();
		numTargetsAlive = targets.Length;
		foreach (var target in targets) {
			target.weaponTarget.SufferedLethalDamage += OnCityTargetDied;
		}

		timeUntilSpawn = 5.0f;
		timeUntilEnemySpawnAccel = TimeToSpawnEnemyAccelInterval;

		if (TEST_PERFORMANCE) {
			gameState = GameState.INTRO;

			for (var i=0; i < 50; i++) {
				SpawnEnemyAt(Enemy, new Vector3(i - 30f, 20f, 60f));
			}
		}
	}

	public void OnCityTargetDied(WeaponTargetable target) {
		numTargetsAlive--;

		if (numTargetsAlive <= 0) {
			GameOver();
		}
	}

	public void ResetGame() {
		VRPreferences.Instance.SaveSettings();
		SceneManager.LoadScene("Game");
	}

	public void GameOver() {
		gameState = GameState.OVER;

		GameOverInfo.SetActive(true);
		GameOverInfo.transform.DOScale(0.01f, 4f).SetEase(Ease.OutBounce).SetDelay(1.0f).From();
	}
	
	// Update is called once per frame
	void Update () {

		if (gameState == GameState.PLAYING) {
			UpdatePlayingState();
		}

		if (Input.GetKey (KeyCode.F))
			Time.timeScale = 16f;
		else if (Time.timeScale > 1f)
			Time.timeScale = 1f;

		if (PlayerTurretHead != null) {
			PlayerTurretHead.transform.rotation = MainCamera.transform.rotation;
			PlayerTurretHead.transform.Rotate(0, -90, 0);
		}
	}

	void UpdatePlayingState()
	{
		timeUntilEnemySpawnAccel -= Time.deltaTime;
		if (timeUntilEnemySpawnAccel <= 0) {
			timeUntilEnemySpawnAccel += TimeToSpawnEnemyAccelInterval;
			TimeToSpawnEnemy.min = Mathf.Max(0.5f, TimeToSpawnEnemy.min - TimeToSpawnEnemyAccel);
			TimeToSpawnEnemy.max = Mathf.Max(1f, TimeToSpawnEnemy.max - TimeToSpawnEnemyAccel);
			EnemySpawnAngleSpread = Mathf.Min(Mathf.PI * 2, EnemySpawnAngleSpread + Mathf.PI / 64);
		}

		timeUntilSpawn -= Time.deltaTime;
		if (timeUntilSpawn <= 0) {
			timeUntilSpawn = TimeToSpawnEnemy.GetRandomValue();
//			Debug.Log("Next wave in: " + timeUntilSpawn);
			SpawnEnemyGroup();
		}

		if (Input.GetKeyDown (KeyCode.J)) {
			SpawnEnemyGroup(Input.GetKey(KeyCode.LeftShift) ? "carrier" : null);
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			KillAllEnemies();
		}

		if (Input.GetKeyDown (KeyCode.G)) {
			GameOver();
		}
	}

//	void OnGUI()
//	{
//		var selectedObject = EventSystem.current.currentSelectedGameObject;
//		GUILayout.Label("Gazing at: "+ (selectedObject ? selectedObject.name : "(null)"));
//	}

	void SpawnEnemyGroup (string forceShip = "")
	{
		var currentWave = Config.Instance.GetWaveById(2);
		var enemyGroup = Config.Instance.SelectEnemyGroupForWave(currentWave);


		CurrentCarrierChance += CarrierChanceIncreasePerWave;
//		Debug.Log("Chance for carrier: " + CurrentCarrierChance);

		if (Random.value < EnemyEmptyChance)
			return;

		var waveCenter = Vector3.zero;
		var polar = Random.value * EnemySpawnAngleSpread - EnemySpawnAngleSpread/2 + Mathf.PI/2;
//		var polar = Random.value * Mathf.PI * 1.2f - Mathf.PI * 0.6f + Mathf.PI/2;
		var elevation = Mathf.Deg2Rad * Random.Range(EnemyMinElevation, EnemyMaxElevation);
		var distance = EnemySpawnDistance.GetRandomValue();


		var enemiesInWave = Random.Range(enemyGroup.Min, enemyGroup.Max+1);
		Debug.Log("Loading prefab "+"Enemies/"+enemyGroup.Prefab);
		var src = Resources.Load<GameObject>("Enemies/"+enemyGroup.Prefab);
		
		if (Random.value <= CurrentCarrierChance || "carrier".Equals(forceShip)) {
//			Debug.Log("Spawning carrier, chance was: " + CurrentCarrierChance);
			CurrentCarrierChance -= CarrierChanceCostToSpawn;

			src = EnemyCarrier;
			enemiesInWave = 1;
			distance *= 2;
		}

		Util.SphericalToCartesian(distance, polar, elevation, out waveCenter);

		for (var i=0; i < enemiesInWave; i++) {
			var delta = Random.onUnitSphere * WAVE_ENEMY_SEPARATION;
			var pos = waveCenter + delta;
			StartCoroutine(SpawnEnemyAfterDelayAt(src, pos, EnemySpawnDelay.GetRandomValue()));
		}
	}

	private IEnumerator SpawnEnemyAfterDelayAt(GameObject src, Vector3 pos, float delay)
	{
		yield return new WaitForSeconds(delay);

		SpawnEnemyAt(src, pos);
	}

	private GameObject SpawnEnemyAt(GameObject src, Vector3 pos)
	{
		var obj = GameObject.Instantiate (src);
		obj.transform.position = pos;
//		obj.transform.LookAt(Vector3.zero);

//		var targetable = obj.GetComponent<PlayerTargetable>();
//		hud.TrackObject(targetable);			
//		targetable.WasLockedOn += OnLockedEnemy;

		return obj;
	}

	private void KillAllEnemies()
	{
		var enemies = FindObjectsOfType<PlayerTargetable>();
		foreach(var enemy in enemies) {
			if (enemy.WasLockedOn != null)
				enemy.WasLockedOn(enemy);
		}
	}

	public void OnTargetableSpawned(PlayerTargetable targetable) {
		hud.TrackObject(targetable);			
		targetable.WasLockedOn += OnLockedEnemy;
	}

	public void OnLockedEnemy(PlayerTargetable target) {
		var shooter = FindShooterClosestToEnemy(target);
		var weaponTarget = target.GetComponent<WeaponTargetable>();

		if (shooter != null)
			shooter.LaunchAgainstTarget(weaponTarget);
	}

	CityShooter FindShooterClosestToEnemy(PlayerTargetable target) {
		var shooters = City.GetComponentsInChildren<CityShooter>();
		CityShooter closest = null;
		var distance = 0f;
		var targetPos = target.transform.position;

		foreach (var shooter in shooters) {
			var shooterTargetable = shooter.GetComponent<WeaponTargetable>();
			if (shooterTargetable.Dead)
				continue;

			var newDistance = (shooter.transform.position - targetPos).sqrMagnitude;
			if (newDistance < distance || closest == null) {
				closest = shooter;
				distance = newDistance;
			}
		}

		return closest;
	}

	public CityTarget FindCityTargetClosestTo(Vector3 position)
	{
		var targets = City.GetComponentsInChildren<CityTarget>();

		CityTarget closest = null;
		var closestRange = float.MaxValue;
		foreach (var target in targets) {
			if (target.Health <= 0)
				continue;

			var range = Vector3.Distance(target.transform.position, position);
			if (range < closestRange)
			{
				closest = target;
				closestRange = range;
			}
		}

		return closest;
	}

	public void TogglePause() {
		Time.timeScale = 1f - Time.timeScale;
	}

}
