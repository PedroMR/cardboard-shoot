using UnityEngine;
using System.Collections;

public class LevelConfig : ScriptableObject
{
	public class WaveConfig : ScriptableObject {

		FloatRange	TimeBetweenWaves;
		float		SpawnAngleSpread;
		FloatRange	SpawnElevation;

		EnemyCandidates[]	Enemies;

	}

	public class EnemyCandidates : ScriptableObject {
		EnemyGroup	EnemyGroup;
		float		Weight;
	}

	public class EnemyGroup : ScriptableObject {
		GameObject	EnemyPrefab;
		FloatRange	Count;
		FloatRange	SpawnDistance;
	}

	[SerializeField]
	public WaveConfig[] waveConfig;
}

