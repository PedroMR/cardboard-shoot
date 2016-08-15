using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreWatcher : MonoBehaviour
{
	public Text label;

	// Use this for initialization
	void Start()
	{
		GameController.Instance.OnScoreChange += OnScoreChange;
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	void OnScoreChange(int newScore)
	{
		label.text = newScore.ToString();
		//TODO speed through numbers, shine, etc
	}
}

