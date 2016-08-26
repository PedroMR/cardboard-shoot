using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateWatcher : MonoBehaviour
{
	public Text WaveDataLabel;
	public Text WarpsLeftDataLabel;

	// Use this for initialization
	void Start()
	{
			
	}
	
	// Update is called once per frame
	void Update()
	{
		WaveDataLabel.text = GameController.Instance.CurrentWaveNumber.ToString();
		WarpsLeftDataLabel.text = GameController.Instance.WarpsLeft.ToString();
	}
}

