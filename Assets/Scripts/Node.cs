using System;
using UnityEngine;

public class Node : MonoBehaviour
{
	public SpriteRenderer Renderer;
	public Color AltColor = Color.red;
	
	[NonSerialized]
	public Vector2Int GridPosition;
	[NonSerialized]
	public Vector2Int NextNode;
	[NonSerialized]
	public int NodeIndex;
	[NonSerialized]
	public bool AltNode = false;

	public void UpdateVisual()
	{
		var dir = NextNode - GridPosition;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		Renderer.color = AltNode ? AltColor : Color.white;
	}
}