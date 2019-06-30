using UnityEngine;

public class CameraControl : Singleton<CameraControl>
{
	public GameObject FollowObject;
	public float CameraShakeSpeed = 1.0f;

	private bool _following = false;
	private Vector3 _velocity;
	private float _shakeAmount;
	private float _shakeDuration;
	private float _startAmount;
	private float _startDuration;

	public void ShakeCamera(float amount, float duration)
	{
		Debug.Log($"shake screen: {amount}, {duration}");
		_shakeAmount = amount;
		_shakeDuration = duration;
		_startAmount = amount;
		_startDuration = duration;
	}
	
	private void Update()
	{
		if(_shakeDuration > 0)
		{
			// choose a random x, y point
			var rotationAmount = Random.insideUnitSphere * _shakeAmount;
			rotationAmount.z = 0;

			// change magnitude of random point based on how far we are through the shake
			var perc = _shakeDuration / _startDuration;
			_shakeAmount = _startAmount * perc;
			_shakeDuration -= Time.deltaTime * CameraShakeSpeed;

			// apply to camera
			transform.localEulerAngles = rotationAmount;
		}
		
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