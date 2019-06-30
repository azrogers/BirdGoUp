using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	public GameState State = GameState.Playing;

	public GameObject PlayingUI;
	public GameObject DeadUI;

	private void Update()
	{
		PlayingUI.SetActive(State == GameState.Playing);
		DeadUI.SetActive(State == GameState.Dead);
		Time.timeScale = State == GameState.Playing ? 1.0f : 0.0f;
		
		if(Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	public enum GameState
	{
		Playing,
		Dead
	}
}