using System;
using UnityEngine;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
	public Text ScoreText;

	private string _scoreString;

	private void Awake()
	{
		_scoreString = ScoreText.text;
	}

	private void OnEnable()
	{
		ScoreText.text =
			_scoreString
				.Replace("{dist}", ScoreManager.Instance.MaxDistance.ToString("N0") + "m")
				.Replace("{score}", ScoreManager.Instance.Score.ToString("N0"));
	}
}