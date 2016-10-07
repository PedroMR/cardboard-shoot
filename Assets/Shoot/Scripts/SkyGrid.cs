using UnityEngine;
using System.Collections;

public class SkyGrid : MonoBehaviour
{
	void Awake()
	{
		var sphere = GetComponent<MeshFilter>();

		var mesh = sphere.mesh;

		var normals = mesh.normals;

		for(var i=0; i < normals.Length; i++) {
			var n = normals[i];
			normals[i] = -n;
		}
		mesh.normals = normals;
	}

}

