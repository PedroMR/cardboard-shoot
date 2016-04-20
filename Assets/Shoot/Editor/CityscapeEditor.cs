using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Cityscape))]
public class CityscapeEditor : Editor {

	// Use this for initialization
	void Start () {
	}

	public override void OnInspectorGUI()
	{
		Cityscape city = (Cityscape)target;
		city.Rows = EditorGUILayout.IntField("Rows", city.Rows);
		city.Cols = EditorGUILayout.IntField("Cols", city.Cols);
		EditorGUILayout.BeginHorizontal();
		city.MinHeight = EditorGUILayout.FloatField("Height range", city.MinHeight);
		city.MaxHeight = EditorGUILayout.FloatField(city.MaxHeight);
		EditorGUILayout.EndHorizontal();
		city.DeadZoneRadius = EditorGUILayout.FloatField("DeadZoneRadius", city.DeadZoneRadius);
		city.BuildingPrefab = (GameObject)EditorGUILayout.ObjectField("Building", city.BuildingPrefab, typeof(GameObject), false);
			
		if (GUILayout.Button("Recreate City")) {
			city.CreateCity();
		}
	}

	
}
