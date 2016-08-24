using System;
using UnityEngine;

public class TimedObjectRepooling : MonoBehaviour
{
	[SerializeField] private float m_TimeOut = 1.0f;

	private void Awake()
	{
		Invoke("Repool", m_TimeOut);
	}


	private void Repool()
	{
		ObjectPool.instance.PoolObject(this.gameObject);
	}
}
