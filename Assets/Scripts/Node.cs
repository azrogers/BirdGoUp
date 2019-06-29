using System;
using UnityEngine;

public class Node : MonoBehaviour
{
	public Vector2Int GridPosition;
	public Vector2Int NextNode;

	public void UpdateRotation()
	{
		var dir = NextNode - GridPosition;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}