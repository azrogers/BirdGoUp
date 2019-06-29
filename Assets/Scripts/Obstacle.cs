using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public int IndexFrom;
	[NonSerialized]
	public Vector2Int PosFrom;
	[NonSerialized]
	public Vector2Int PosBlockingTo;

	private void Update()
	{
		if(LevelGenerator.Instance.HighestNodeReached > IndexFrom)
		{
			Destroy(gameObject);
		}
	}
}