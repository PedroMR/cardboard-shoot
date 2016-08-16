using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreWatcher : MonoBehaviour
{
	public Text label;

	public Text bestScoreLabel;

	public static int BestScore = 0;

	// Use this for initialization
	void Start()
	{
		GameController.Instance.OnScoreChange += OnScoreChange;
		bestScoreLabel.text = BestScore.ToString();
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	void OnScoreChange(int newScore)
	{
		label.text = newScore.ToString();
		//TODO speed through numbers, shine, etc


		if (newScore > BestScore) {
			BestScore = newScore;
			bestScoreLabel.text = newScore.ToString();
		}
	}
}

