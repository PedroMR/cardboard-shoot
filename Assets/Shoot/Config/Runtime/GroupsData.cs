using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
///
[System.Serializable]
public class GroupsData
{
	[SerializeField]
	string _ID;
	public string ID { get {return _ID; } set { _ID = value;} }
	
	[SerializeField]
	int _Min;
	public int Min { get {return _Min; } set { _Min = value;} }
	
	[SerializeField]
	int _Max;
	public int Max { get {return _Max; } set { _Max = value;} }
	
	[SerializeField]
	string _Prefab;
	public string Prefab { get {return _Prefab; } set { _Prefab = value;} }
	
	[SerializeField]
	float[] _SpawnElevation = new float[0];
	public float[] SpawnElevation { get {return _SpawnElevation; } set { _SpawnElevation = value;} }
	
	[SerializeField]
	float[] _SpawnDistance = new float[0];
	public float[] SpawnDistance { get {return _SpawnDistance; } set { _SpawnDistance = value;} }
	
}