using System;
using UnityEngine;

public class TimedObjectRepooling : MonoBehaviour
{
	[SerializeField] private float m_TimeOut = 1.0f;

	private void OnEnable()
	{
//		Debug.Log("Repooling " + gameObject.name + " after " + m_TimeOut + "s");
		Invoke("Repool", m_TimeOut);
	}


	private void Repool()
	{
//		Debug.Log("Repooling " + gameObject.name);
		ObjectPool.instance.PoolObject(this.gameObject);
	}
}
