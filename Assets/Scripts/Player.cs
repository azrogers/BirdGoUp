using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player>
{
	public float JumpVelocity = 100.0f;
	public float Gravity = 1.0f;
	public float AirResistance = 0.5f;
	public float BoostDuration = 0.5f;
	public float AltDelayTime = 0.5f;
	public PlayerSpriteHandler SpriteHandler;
	
	private bool _firstJump = true;
	private bool _canJump = true;
	private Vector2 _velocity;
	private Vector2Int _gridPos = Vector2Int.zero;
	private Vector3 _boostStart;
	private Vector3 _boostDest;
	private float _boostTime;

	public bool FirstJump => _firstJump;

	private void HandleJump()
	{
		if(_firstJump)
		{
			_firstJump = false;
		}

		var posChange = Vector2Int.zero;
		if(Input.GetKey(KeyCode.W))
		{
			posChange += Vector2Int.up;
		}
		if(Input.GetKey(KeyCode.S))
		{
			posChange += Vector2Int.down;
		}
		if(Input.GetKey(KeyCode.A))
		{
			posChange += Vector2Int.left;
		}
		if(Input.GetKey(KeyCode.D))
		{
			posChange += Vector2Int.right;
		}

		if(posChange == Vector2Int.zero)
		{
			return;
		}

		var newPos = _gridPos + posChange;
		_boostStart = transform.localPosition;
		_boostDest = LevelGenerator.Instance.GetPosFor(newPos);
		_boostTime = BoostDuration;
		_gridPos = newPos;
	}

	private void Update()
	{
		if(_canJump && Input.GetKeyDown(KeyCode.Space))
		{
			HandleJump();
			AudioManager.Instance.PlayWhoosh();
			_canJump = false;
			_velocity = Vector2.zero;
		}

		if(Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		_velocity *= Vector2.one - (new Vector2(AirResistance, AirResistance) * Time.deltaTime);

		if(_boostTime > 0)
		{
			var boostPos = Util.Lerp(_boostDest, _boostStart, _boostTime / BoostDuration, Ease.InQuad);
			var diff = boostPos.x - transform.localPosition.x;
			transform.localPosition = boostPos;
			var rot = 0.0f;
			if(!Mathf.Approximately(diff, 0.0f))
			{
				rot = diff > 0.0f ? 1.0f : -1.0f;
			}
			SpriteHandler.UpdatePlayerRotation(rot);
		}
		else
		{
			if(!_firstJump)
			{
				_velocity -= new Vector2(0, Gravity) * Time.deltaTime;
			}

			transform.Translate(_velocity);

			var localPosition = transform.localPosition;
			_gridPos = new Vector2Int(
				Mathf.RoundToInt(localPosition.x / LevelGenerator.Instance.WidthMult),
				Mathf.RoundToInt(localPosition.y / LevelGenerator.Instance.HeightMult)
			);
			
			SpriteHandler.UpdatePlayerRotation(0.0f);
		}

		_boostTime -= Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Node"))
		{
			var nc = other.GetComponent<Node>();
			_gridPos = nc.GridPosition;
			AudioManager.Instance.PlayCollect();
			if(nc.NodeIndex > LevelGenerator.Instance.HighestNodeReached)
			{
				LevelGenerator.Instance.HighestNodeReached = nc.NodeIndex;
			}

			_canJump = true;
			Destroy(other.gameObject);
		}
		else if(other.CompareTag("Obstacle"))
		{
			_boostTime = 0;
		}
	}
}
