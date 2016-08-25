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
	int wave;
	public int Wave { get {return wave; } set { wave = value;} }
	
	[SerializeField]
	string[] groupids = new string[0];
	public string[] Groupids { get {return groupids; } set { groupids = value;} }
	
	[SerializeField]
	int[] groupweights = new int[0];
	public int[] Groupweights { get {return groupweights; } set { groupweights = value;} }
	
	[SerializeField]
	float[] timebetweenwaves = new float[0];
	public float[] Timebetweenwaves { get {return timebetweenwaves; } set { timebetweenwaves = value;} }
	
	[SerializeField]
	float spawnanglespread;
	public float Spawnanglespread { get {return spawnanglespread; } set { spawnanglespread = value;} }
	
	[SerializeField]
	float[] spawnelevation = new float[0];
	public float[] Spawnelevation { get {return spawnelevation; } set { spawnelevation = value;} }
	
}