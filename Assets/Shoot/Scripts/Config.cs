using UnityEngine;
using System.Collections;

public class Config : ScriptableObject {

	public Groups EnemyGroups;

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
}
