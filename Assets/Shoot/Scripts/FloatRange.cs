﻿using UnityEngine;

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

	/**
	 * Returns an int value between the floored values of min and max (inclusive!)
	 */
	public int GetIntValue() {
		return UnityEngine.Random.Range((int)min, (int)max+1);
	}

	public static float GetValue(float[] range) {
		return Random.Range(range[0], range[1]);
	}
}

