using UnityEngine;
using System.Collections;

public class Config : ScriptableObject {

	public Groups EnemyGroups;
	public Waves EnemyWaves;

	public int startingWave = 0;

	private static Config _instance;

	public Config() {
		_instance = this;
	}

	public static Config Instance { get { 
		if (_instance == null) {
				_instance = Resources.Load<Config>("Game Config");
		}
		return _instance; 
	}}

	public GroupsData GetGroupById(string ID) {
		foreach(var group in EnemyGroups.dataArray) {
			if (string.Equals(group.ID, ID)) {
				return group;
			}
		}

		return null;
	}

	public WavesData GetWaveById(int waveId) {
		foreach (var wave in EnemyWaves.dataArray) {
			if (wave.Wave == waveId) {
				return wave;
			}
		}

		return null;
	}

	public GroupsData SelectEnemyGroupForWave(WavesData wave) {
		var total = 0;

		foreach (var group in wave.GroupWeights) {
			total += group;
		}

		var value = Random.value * total;
		for (var i=0; i < wave.GroupWeights.Length; i++) {
			value -= wave.GroupWeights[i];
			if (value <= 0) {
				return GetGroupById(wave.GroupIDs[i]);
			}
		}

		Debug.LogError("Error selecting random enemy group in wave " + wave.Wave);
		return null;
	}
}
