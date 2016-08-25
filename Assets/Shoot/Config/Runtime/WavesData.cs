using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
///
[System.Serializable]
public class WavesData
{
	[SerializeField]
	int _Wave;
	public int Wave { get {return _Wave; } set { _Wave = value;} }
	
	[SerializeField]
	string[] _GroupIDs = new string[0];
	public string[] GroupIDs { get {return _GroupIDs; } set { _GroupIDs = value;} }
	
	[SerializeField]
	int[] _GroupWeights = new int[0];
	public int[] GroupWeights { get {return _GroupWeights; } set { _GroupWeights = value;} }
	
	[SerializeField]
	float[] _TimeBetweenWaves = new float[0];
	public float[] TimeBetweenWaves { get {return _TimeBetweenWaves; } set { _TimeBetweenWaves = value;} }
	
	[SerializeField]
	float _SpawnAcceleration;
	public float SpawnAcceleration { get {return _SpawnAcceleration; } set { _SpawnAcceleration = value;} }
	
	[SerializeField]
	float _SpawnAngleSpread;
	public float SpawnAngleSpread { get {return _SpawnAngleSpread; } set { _SpawnAngleSpread = value;} }
	
	[SerializeField]
	int _MaxGroupsSpawned;
	public int MaxGroupsSpawned { get {return _MaxGroupsSpawned; } set { _MaxGroupsSpawned = value;} }
	
}