using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {

	public GameObject RocketPrefab;

	private static Config _instance;

	public Config() {
		_instance = this;
	}

	public static Config Instance { get { return _instance; }}

}
