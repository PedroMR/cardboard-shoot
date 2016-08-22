using UnityEngine;

public class FloatRange
{
	public float min, max;

	/**
	 * Creates random numbers between min and max (inclusive).
	 */
	public FloatRange(float min, float max) {
		this.min = min;
		this.max = max;
	}
		
	public float GetRandomValue() {
		//TODO deterministic if needed
		return UnityEngine.Random.Range(min, max);
	}
}

