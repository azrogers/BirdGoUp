using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
	public Text ScoreText;
	public int ScorePerNode = 100;
	public int MaxCombo = 10;

	private int _sequenceCombo = 0;
	private int _score = 0;
	private int _lastIndexHit = 0;
	
	public void HitNode(int index)
	{
		if(index == _lastIndexHit + 1)
		{
			_sequenceCombo++;
		}
		else
		{
			_sequenceCombo = 0;
		}

		_score += ScorePerNode;
		if(_sequenceCombo > 1)
		{
			var comboNumber = Mathf.Min(MaxCombo, _sequenceCombo) - 1;
			_score += (int)Mathf.Pow(1.5f, comboNumber) * ScorePerNode;
		}

		_lastIndexHit = index;
	}

	private void Update()
	{
		ScoreText.text = _score.ToString().PadLeft(6, '0');
	}
}