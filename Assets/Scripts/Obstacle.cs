using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public SpriteRenderer Renderer;
	
	public int IndexFrom;
	[NonSerialized]
	public Vector2Int PosFrom;
	[NonSerialized]
	public Vector2Int PosBlockingTo;

	private BoxCollider2D _collider;
	private bool _dead = false;

	private void Kill()
	{
		_dead = true;
		Destroy(_collider);
		var rb = gameObject.AddComponent<Rigidbody2D>();
		rb.simulated = true;
		rb.gravityScale = 1.0f;
		rb.AddForce((Vector2.up + (UnityEngine.Random.value > 0.5f ? Vector2.left : Vector2.right)) * 10);
		rb.AddTorque(UnityEngine.Random.value > 0.5f ? 100.0f : -100.0f);
	}
	
	private void Awake()
	{
		_collider = GetComponent<BoxCollider2D>();
	}

	private void Update()
	{
		if(LevelGenerator.Instance.HighestNodeReached > IndexFrom && !_dead)
		{
			Kill();
		}

		if(_dead && !Renderer.isVisible)
		{
			Destroy(gameObject);
		}
	}
}