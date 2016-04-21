using UnityEngine;

public class Util
{
	public Util()
	{
	}


	public static void DestroyASAP(Object o)
	{
		if (Application.isPlaying)
		{
			GameObject.Destroy(o);
		}
		else if (Application.isEditor)
		{
			GameObject.DestroyImmediate(o);
		}
	}

	public static void SphericalToCartesian(float radius, float polar, float elevation, out Vector3 outCart){
		float a = radius * Mathf.Cos(elevation);
		outCart.x = a * Mathf.Cos(polar);
		outCart.y = radius * Mathf.Sin(elevation);
		outCart.z = a * Mathf.Sin(polar);
	}
}
