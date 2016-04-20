using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LockProgressUI : MonoBehaviour {
	public Image image;

	/**
	 * <param name="amount">from 0 to 1, inclusive</param>
	 */
	public void SetLockProgress(float amount)
	{
		if (image != null)
		{
			image.fillAmount = amount;
		}
	}
}
