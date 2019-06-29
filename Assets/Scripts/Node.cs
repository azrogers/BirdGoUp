using System;
using UnityEngine;

public class Node : MonoBehaviour
{
	public SpriteRenderer Renderer;
	
	[NonSerialized]
	public Vector2Int GridPosition;
	[NonSerialized]
	public Vector2Int NextNode;
	[NonSerialized]
	public int NodeIndex;

	private BoxCollider2D _collider;
	private bool _dead = false;
	
	public void Kill()
	{
		_dead = true;
		Destroy(_collider);
		var rb = gameObject.AddComponent<Rigidbody2D>();
		rb.simulated = true;
		rb.gravityScale = 1.0f;
		rb.AddForce((Vector2.up + (UnityEngine.Random.value > 0.5f ? Vector2.left : Vector2.right)) * 100);
		rb.AddTorque(UnityEngine.Random.value > 0.5f ? 1000.0f : -1000.0f);
	}
	
	public void UpdateVisual()
	{
		var dir = NextNode - GridPosition;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	private void Awake()
	{
		_collider = GetComponent<BoxCollider2D>();
	}

	private void Update()
	{
		// if we're dead, destroy when off screen
		if(_dead && !Renderer.isVisible)
		{
			Destroy(gameObject);
		}
	}
}