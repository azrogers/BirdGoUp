using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player>
{
	public float JumpVelocity = 100.0f;
	public float Gravity = 1.0f;
	public float AirResistance = 0.5f;
	public float BoostDuration = 0.5f;
	
	private bool _firstJump = true;
	private bool _canJump = true;
	private Vector2 _velocity;
	private Vector2 _boostVelocity;
	private float _boostTime;

	public bool FirstJump => _firstJump;

	private Vector2 HandleJump()
	{
		if(!_canJump)
		{
			return Vector2.zero;
		}
		
		if(_firstJump)
		{
			_firstJump = false;
		}

		var dirVert = Vector2.zero;
		var dirHoriz = Vector2.zero;
		if(Input.GetKey(KeyCode.W))
		{
			dirVert += Vector2.up;
		}
		if(Input.GetKey(KeyCode.S))
		{
			dirVert += Vector2.down;
		}
		if(Input.GetKey(KeyCode.A))
		{
			dirHoriz += Vector2.left;
		}
		if(Input.GetKey(KeyCode.D))
		{
			dirHoriz += Vector2.right;
		}

		var accel = JumpVelocity * dirVert;
		accel += JumpVelocity * dirHoriz;
		return accel;
	}
	
	private void Update()
	{
		if(_canJump && Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("jumping");
			_boostVelocity = HandleJump();
			_boostTime = BoostDuration;
			_canJump = false;
			_velocity = Vector2.zero;
		}

		if(Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		// if there's any acceleration, reset velocity
		// that's what we're going for here
		/*if(acceleration.sqrMagnitude > 0)
		{
			_velocity = Vector2.zero;
		}*/

		//_velocity += acceleration
		_velocity *= Vector2.one - (new Vector2(AirResistance, AirResistance) * Time.deltaTime);
		
		if(!_firstJump && _boostTime <= 0)
		{
			_velocity -= new Vector2(0, Gravity) * Time.deltaTime;
		}

		var boostVelLerped = Util.Lerp(Vector2.zero, _boostVelocity, _boostTime / BoostDuration, Ease.InQuad);
		transform.Translate(_velocity + boostVelLerped);

		_boostTime -= Time.deltaTime;
		if(_boostTime <= 0)
		{
			_boostVelocity = Vector2.zero;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Node"))
		{
			_canJump = true;
			Destroy(other.gameObject);
		}
	}
}
