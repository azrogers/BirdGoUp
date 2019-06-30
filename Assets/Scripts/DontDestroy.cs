using System;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
	private static bool _alreadyCreated = false;
	private void Awake()
	{
		if(_alreadyCreated)
		{
			Destroy(gameObject);
			return;
		}
		
		DontDestroyOnLoad(gameObject);
		_alreadyCreated = true;
	}
}