using UnityEngine;
using System.Collections;

public class Cityscape : MonoBehaviour {
	public GameObject BuildingPrefab;
	public GameObject container;
	public GameObject avoid;

	public int Rows = 40;
	public int Cols = 40;
	public float MinHeight = 1;
	public float MaxHeight = 3;
	public float DeadZoneRadius = 10f;


	public void CreateCity() {
		var destroyed = 0;
		if (container == null)
			container = gameObject;
		
		for (var i = container.transform.childCount-1; i>0; i--) {
			var b = container.transform.GetChild(i);
			Util.DestroyASAP(b.gameObject);
			destroyed++;
		}

		if (BuildingPrefab != null) {
			var renderer = container.GetComponent<Renderer>();

			if (renderer != null) {
				var bounds = renderer.bounds;

				var dx = bounds.size.x / (Cols);
				var dz = bounds.size.z / (Rows);
				var x0 = bounds.min.x + dx/2;
				var z0 = bounds.min.z + dz/2;
				var z = z0;
				var pos = container.transform.position;

				for (var i=0; i < Rows; i++, z+=dz) {
					pos.z = z;
					var x = x0;
					for (var j = 0; j < Cols; j++, x += dx) {
						pos.x = x;

//						if (Vector3.Distance(pos, bounds.center) < DeadZoneRadius)
//							continue;
						
						if (Physics.CheckSphere(pos, DeadZoneRadius))
							continue;

						var newBuilding = (GameObject)GameObject.Instantiate(BuildingPrefab, pos, Quaternion.identity);
						newBuilding.transform.parent = container.transform;
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

	void Awake() {
		container = this.gameObject;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
