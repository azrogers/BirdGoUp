using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public GameObject FollowObject;

	private bool _following = false;
	private Vector3 _velocity;
	
	private void Awake()
	{
		
	}

	private void Update()
	{
		if(!_following && !Player.Instance.FirstJump)
		{
			_following = true;
		}

		if(_following)
		{
			var position = transform.position;
		
			var newPos = Vector3.SmoothDamp(
				position, 
				FollowObject.transform.position, 
				ref _velocity, 
				0.1f);

			position.x = newPos.x;
			position.y = newPos.y;
			transform.position = position;
		}
	}
}