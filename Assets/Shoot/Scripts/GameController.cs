using UnityEngine;
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
	private FloatRange TimeToSpawnEnemy;

	public FloatRange EnemySpawnDelay = new FloatRange(0f, 1f);

	public float GROUP_ENEMY_SEPARATION = 10f;

	private int numTargetsAlive;

	private int currentWaveId = 1;
	private WavesData CurrentWave;
	private int groupsLeftToSpawn;

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

	public int WarpsLeft {
		get { return groupsLeftToSpawn; }
	}
	public int CurrentWaveNumber {
		get { return currentWaveId; }
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

		Score = 0;
		GameOverInfo.SetActive(false);

		SetWave(1);
		gameState = GameState.PLAYING;

		var targets = City.GetComponentsInChildren<CityTarget>();
		numTargetsAlive = targets.Length;
		foreach (var target in targets) {
			target.weaponTarget.SufferedLethalDamage += OnCityTargetDied;
		}

		timeUntilSpawn = 5.0f;

		if (TEST_PERFORMANCE) {
			gameState = GameState.INTRO;

			for (var i=0; i < 50; i++) {
				SpawnEnemyAt(Enemy, new Vector3(i - 30f, 20f, 60f));
			}
		}
	}

	private void SetWave(int waveId) {
		var config = Config.Instance;

		var newWave = config.GetWaveById(waveId);
		if (newWave != null) {
			CurrentWave = newWave;
			Debug.Log("Wave "+CurrentWave.Wave+" begins.");
		} else {
			Debug.Log("No wave " + waveId + "  -- staying on our wave config "+CurrentWave.Wave);
		}
		currentWaveId = waveId;

		TimeToSpawnEnemy = new FloatRange(CurrentWave.TimeBetweenWaves);
		groupsLeftToSpawn = CurrentWave.MaxGroupsSpawned;
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
		var spawnAccelerationAmount = CurrentWave.SpawnAcceleration * Time.deltaTime;
		if (spawnAccelerationAmount != 0) {
			TimeToSpawnEnemy.min = Mathf.Max(0.5f, TimeToSpawnEnemy.min - spawnAccelerationAmount);
			TimeToSpawnEnemy.max = Mathf.Max(1f, TimeToSpawnEnemy.max - spawnAccelerationAmount);
		}

		if (groupsLeftToSpawn > 0) {
			timeUntilSpawn -= Time.deltaTime;
			if (timeUntilSpawn <= 0) {
				timeUntilSpawn = TimeToSpawnEnemy.GetRandomValue();
//				Debug.Log("Next wave in: " + timeUntilSpawn+" out of "+TimeToSpawnEnemy.ToString());
				SpawnEnemyGroup();
			}
		} else {
			var allEnemies = FindAllEnemies();
			var enemiesLeft = allEnemies != null ? allEnemies.Length : 0;
//			Debug.Log("Enemies left: " + enemiesLeft);
			if (enemiesLeft == 0) {
				SetWave(currentWaveId + 1);
			}
		}

		if (Input.GetKeyDown (KeyCode.J)) {
			SpawnEnemyGroup(Input.GetKey(KeyCode.LeftShift) ? "carrier" : null);
		}

		if (Input.GetKeyDown(KeyCode.W)) {
			SetWave(currentWaveId + 1);
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			KillAllEnemies();
		}

		if (Input.GetKeyDown (KeyCode.G)) {
			GameOver();
		}
	}

	void SpawnEnemyGroup (string debugForceGroup = null)
	{
		var enemyGroup = Config.Instance.SelectEnemyGroupForWave(CurrentWave);

		if (debugForceGroup != null) {
			enemyGroup = Config.Instance.GetGroupById(debugForceGroup);
		}

		groupsLeftToSpawn--;

		var angleSpreadRad = Mathf.Deg2Rad * CurrentWave.SpawnAngleSpread;
		var elevation = Mathf.Deg2Rad * FloatRange.GetValue(enemyGroup.SpawnElevation);
		var polar = Random.value * angleSpreadRad - angleSpreadRad/2 + Mathf.PI/2; // last add is to rotate to Z axis being forward (match VR center)
		var distance = FloatRange.GetValue(enemyGroup.SpawnDistance);
		Vector3 groupCenter;
		Util.SphericalToCartesian(distance, polar, elevation, out groupCenter);
//		Debug.Log("Spawning Group " + enemyGroup.ID + " at distance " + distance + " elev " + elevation*Mathf.Rad2Deg + " angle " + polar*Mathf.Rad2Deg);


		var enemiesInWave = Random.Range(enemyGroup.Min, enemyGroup.Max+1);

		var src = Resources.Load<GameObject>("Enemies/"+enemyGroup.Prefab);
		if (src == null) {
			Debug.LogWarning("Wave "+CurrentWave.Wave+": Error loading prefab "+"Enemies/"+enemyGroup.Prefab+" for group "+enemyGroup.ID);
			return;
		}

		for (var i=0; i < enemiesInWave; i++) {
			var delta = Random.onUnitSphere * GROUP_ENEMY_SEPARATION;
			var pos = groupCenter + delta;
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
		var enemies = FindAllEnemies();
		foreach(var enemy in enemies) {
			if (enemy.WasLockedOn != null)
				enemy.WasLockedOn(enemy);
		}
	}

	private PlayerTargetable[] FindAllEnemies()
	{
		return FindObjectsOfType<PlayerTargetable>();
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
