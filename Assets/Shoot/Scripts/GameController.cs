﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using SWS;

public class GameController : MonoBehaviour {
	public TargetingHUD hud;
	public GameObject Enemy;
	public GameObject RocketPrefab;
	public GameObject PlayerTurretSpawn;
	public GameObject MainCamera;
	public GameObject PlayerTurretHead;
	public GameObject City;
	public GameObject GameOverInfo;

	private float timeUntilSpawn;
	public FloatRange TimeToSpawnEnemy = new FloatRange(4.0f, 8.0f);
	public float EnemySpawnDistance = 60f;
	public float EnemyMinElevation = 15f;
	public float EnemyMaxElevation = 45f;
	public float EnemyEmptyChance = 0.1f;

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
		Score = 0;
		GameOverInfo.SetActive(false);
		gameState = GameState.PLAYING;

		var targets = City.GetComponentsInChildren<CityTarget>();
		numTargetsAlive = targets.Length;
		foreach (var target in targets) {
			target.weaponTarget.SufferedLethalDamage += OnCityTargetDied;
		}

		timeUntilSpawn = 5.0f;
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
		timeUntilSpawn -= Time.deltaTime;
		if (timeUntilSpawn <= 0) {
			timeUntilSpawn = TimeToSpawnEnemy.GetRandomValue();
			SpawnEnemyWave();
		}

		if (Input.GetKeyDown (KeyCode.J)) {
			SpawnEnemyWave();
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

	void SpawnEnemyWave ()
	{
		if (Random.value < EnemyEmptyChance)
			return;
		
		var waveCenter = Vector3.zero;
		var polar = Random.value * Mathf.PI * 1.2f - Mathf.PI * 0.6f;
		var elevation = Mathf.Deg2Rad * Random.Range(EnemyMinElevation, EnemyMaxElevation);
		Util.SphericalToCartesian(EnemySpawnDistance, polar, elevation, out waveCenter);

		var enemiesInWave = Random.Range(WAVE_MIN_ENEMIES,WAVE_MAX_ENEMIES+1);
		var src = Enemy;
		for (var i=0; i < enemiesInWave; i++) {
			var obj = GameObject.Instantiate (src);
			hud.TrackObject (obj.GetComponent<PlayerTargetable> ());
			
			var delta = Random.onUnitSphere * WAVE_ENEMY_SEPARATION;
			var pos = waveCenter + delta;
			obj.transform.position = pos;
			obj.transform.LookAt(Vector3.zero);

			var targetable = obj.GetComponent<PlayerTargetable>();
			targetable.WasLockedOn += OnLockedEnemy;
		}
	}

	public void OnLockedEnemy(PlayerTargetable target) {
		var shooter = FindShooterClosestToEnemy(target);
		var weaponTarget = target.GetComponent<WeaponTargetable>();
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

	public void TogglePause() {
		Time.timeScale = 1f - Time.timeScale;
	}

}
