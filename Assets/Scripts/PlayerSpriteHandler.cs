using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteHandler : MonoBehaviour
{
	public Sprite[] MovementSprites;

	// -1 = all the way left, 1 = all the way right
	private float _playerRotation = 0.0f;
	private float _targetPlayerRotation = 0.0f;
	private float _velocity = 0.0f;
	private SpriteRenderer _renderer;

	public void UpdatePlayerRotation(float rotationNormalized)
	{
		_targetPlayerRotation = Mathf.Clamp(rotationNormalized, -1.0f, 1.0f);
	}

	private void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		_playerRotation = Mathf.SmoothDamp(_playerRotation, _targetPlayerRotation, ref _velocity, 0.2f);
		var index = Mathf.FloorToInt((1.0f - Mathf.Abs(_playerRotation)) * MovementSprites.Length);
		if(index >= MovementSprites.Length)
		{
			index = MovementSprites.Length - 1;
		}
		_renderer.sprite = MovementSprites[index];
		_renderer.flipX = _playerRotation < 0.0f;
	}
}