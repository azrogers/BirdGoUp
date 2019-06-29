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
	
	private bool _firstJump = true;
	private bool _canJump = true;
	private Vector2 _velocity;
	private Vector2Int _gridPos = Vector2Int.zero;
	private Vector3 _boostStart;
	private Vector3 _boostDest;
	private float _boostTime;
	private float _altDelay;
	private bool _waitingForAlt = false;

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
		if(_waitingForAlt && Input.GetKeyDown(KeyCode.P))
		{
			_altDelay = 0;
			_waitingForAlt = false;
			_canJump = true;
		}
		
		if(_canJump && Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("jumping");
			HandleJump();
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
			transform.localPosition = boostPos;
		}
		else
		{
			if(!_firstJump && _altDelay <= 0)
			{
				_velocity -= new Vector2(0, Gravity) * Time.deltaTime;
			}

			transform.Translate(_velocity);

			var localPosition = transform.localPosition;
			_gridPos = new Vector2Int(
				Mathf.RoundToInt(localPosition.x / LevelGenerator.Instance.WidthMult),
				Mathf.RoundToInt(localPosition.y / LevelGenerator.Instance.HeightMult)
			);
		}

		_altDelay -= Time.deltaTime;
		_boostTime -= Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Node"))
		{
			var nc = other.GetComponent<Node>();
			_gridPos = nc.GridPosition;
			if(nc.NodeIndex > LevelGenerator.Instance.HighestNodeReached)
			{
				LevelGenerator.Instance.HighestNodeReached = nc.NodeIndex;
			}

			if(nc.AltNode)
			{
				_altDelay = AltDelayTime;
				_waitingForAlt = true;
			}
			else
			{
				_canJump = true;
			}
			Destroy(other.gameObject);
		}
	}
}
