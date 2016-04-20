using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CityScape : MonoBehaviour {
	public GameObject BuildingPrefab;
	public Renderer renderer;

	public int Rows = 20;
	public int Cols = 20;
	public float MinHeight = 1;
	public float MaxHeight = 3;

	// Use this for initialization
	void Start () {
		CreateCity();
	}

	void CreateCity() {
		if (BuildingPrefab != null) {
			if (this.renderer != null) {
				var bounds = renderer.bounds;

				var dx = bounds.size.x / (Cols);
				var dz = bounds.size.z / (Rows);
				var x0 = bounds.min.x + dx/2;
				var z0 = bounds.min.z + dz/2;
				var z = z0;
				var pos = transform.position;

				for (var i=0; i < Rows; i++, z+=dz) {
					pos.z = z;
					var x = x0;
					for (var j = 0; j < Cols; j++, x += dx) {
						pos.x = x;
						var newBuilding = (GameObject)GameObject.Instantiate(BuildingPrefab, pos, Quaternion.identity);
						newBuilding.transform.parent = transform;
						var scale = newBuilding.transform.localScale;
						scale.y *= Random.Range(MinHeight, MaxHeight);
						newBuilding.transform.localScale = scale;
						var r = newBuilding.GetComponent<Renderer>();
						var lp = newBuilding.transform.localPosition;
						lp.y -= r.bounds.min.y;
						newBuilding.transform.localPosition = lp;
					}
				}

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.C)) {
			for (var i = 0; i < transform.childCount; i++) {
				var b = transform.GetChild(i);
				GameObject.Destroy(b.gameObject);
			}
			CreateCity();
		}
	}
}
