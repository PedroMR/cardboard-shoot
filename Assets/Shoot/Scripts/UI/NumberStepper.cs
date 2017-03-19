using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumberStepper : MonoBehaviour
{
	public Button lessButton;
	public Button moreButton;
	public Text valueText;

	public int minValue = 1;
	public int maxValue = 10;
	public int value;

	// Use this for initialization
	void Start()
	{
		lessButton.onClick.AddListener(OnLessPressed);
		moreButton.onClick.AddListener(OnMorePressed);
		SetValue(value);
	}

	public void OnLessPressed()
	{
		SetValue(value - 1);
	}

	public void OnMorePressed()
	{
		SetValue(value + 1);
	}

	void SetValue(int newValue)
	{
		value = Mathf.Clamp(newValue, minValue, maxValue);
		valueText.text = value.ToString();
	}

	// Update is called once per frame
	void Update()
	{
	
	}
}

