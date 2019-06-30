using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
	private void Awake()
	{
		var height = Screen.currentResolution.height * 0.9f;
		var width = (10.0f / 16.0f) * height;
		Screen.SetResolution((int)width, (int)height, false);
		SceneManager.LoadScene("MainScene");
	}
}