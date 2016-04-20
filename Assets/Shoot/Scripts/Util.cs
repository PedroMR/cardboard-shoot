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
}
