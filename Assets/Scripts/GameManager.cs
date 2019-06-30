using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	public GameState State = GameState.Title;

	public GameObject PlayingUI;
	public GameObject DeadUI;
	public GameObject TitleUI;

	private static bool _playedTitle = false;

	public void Play()
	{
		State = GameState.Playing;
		_playedTitle = true;
	}

	private void Awake()
	{
		if(_playedTitle)
		{
			State = GameState.Playing;
		}
	}

	private void Update()
	{
		if(State != GameState.Title)
		{
			_playedTitle = true;
		}
		
		TitleUI.SetActive(State == GameState.Title);
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
		Dead,
		Title
	}
}