using System;
using UnityEngine;

/// <summary>
/// A component that controls the player's movement.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	private int _freezeCount = 0;

	private bool _isGrounded = true;
	private float _coyoteTimer = 0f;

	private float _velocityY;
	private CharacterController _controller;
	private GroundedChecker _checker;

	#region Unity fields
	[SerializeField]
	private MainCamera _camera;
	[SerializeField]
	private Transform _body;

	[Header("Movement")]
	[SerializeField]
	[Tooltip("Minimal movement speed of the player.")]
	private float _minSpeed = 1f;
	[SerializeField]
	[Tooltip("Movement speed of the player.")]
	private ModifiableFloat _speed = new(12f, minValue: 0f);

	[Header("Gravity")]
	[SerializeField]
	private ModifiableFloat _gravity = new(9.8f, minValue: 0f);
	[SerializeField, Min(float.Epsilon)]
	private float _coyoteTime = 0.1f;
	[SerializeField, Min(0f)]
	private float _maxVelocityY = 20f;
	[SerializeField]
	private LayerMask _groundLayers = 1 << 0;
	[SerializeField]
	private float _idleDownforce = 0.1f;
	#endregion

	#region Properties
	public CharacterController controller => _controller;

	/// <summary>
	/// Gets a modifiable movement speed of the player.
	/// </summary>
	public ModifiableFloat movementSpeed => _speed;

	/// <summary>
	/// Gets a modifiable gravity (vertical speed acceleration) of the player.
	/// </summary>
	public ModifiableFloat gravity => _gravity;

	/// <summary>
	/// Gets/sets the input axis. The magnitude of this axis must
	/// be less than 1.
	/// </summary>
	public Vector2 axis { get; set; }

	/// <summary>
	/// Gets a camera assigned to this component.
	/// </summary>
	public MainCamera mainCamera => _camera;

	/// <summary>
	/// Gets a direction of movement.
	/// </summary>
	public Vector3 direction => Mathf.Approximately(axis.sqrMagnitude, 0f) ?
		_body.forward : _camera.AxisToDirection(axis);

	/// <summary>
	/// Gets a boolean value that determines whether the player is grounded.
	/// </summary>
	public bool isGrounded => _isGrounded;

	/// <summary>
	/// Gets a boolean value that determines whether the movement is freezed.
	/// </summary>
	public bool isFreezed => _freezeCount > 0;

	/// <summary>
	/// Gets a number of "freezes" set on this movement.
	/// </summary>
	public int freezeCount => _freezeCount;

	/// <summary>
	/// Gets a current speed of the player.
	/// </summary>
	public float currentSpeed { get; private set; }

	public float verticalSpeed => _velocityY;

	public Vector3 globalVelocity { get; private set; }
	#endregion

	#region Events
	/// <summary>
	/// An event that is triggered when the player touches a ground.
	/// </summary>
	public event Action<PlayerMovement> grounded;
	/// <summary>
	/// An event that is triggered when the player in a moment when it
	/// is no longer touching a ground.
	/// </summary>
	public event Action<PlayerMovement> elevated;

	/// <summary>
	/// An event that is triggered when the <see cref="Freeze" /> method is called.
	/// </summary>
	public event Action<PlayerMovement> freezeSet;
	#endregion

	private void Awake()
	{
		_controller = GetComponent<CharacterController>();
		_checker = this.TryGetInChildren<GroundedChecker>();
	}

	private void OnEnable()
	{
		_isGrounded = true;
		_coyoteTimer = -1f;
		_velocityY = 0f;
		CheckIfGrounded();
	}

	private void SetGrounded()
	{
		_isGrounded = true;
		grounded?.Invoke(this);

		_velocityY = 0f;
		_coyoteTimer = -1f;
	}

	private void SetMidfly()
	{
		_coyoteTimer = -1f;
		_isGrounded = false;
		elevated?.Invoke(this);
	}

	private void CheckIfGrounded()
	{
		bool previousGrounded = _coyoteTimer > 0f || _isGrounded;
		bool newGrounded = _checker.isGrounded;

		// If the grounded state has changed
		if (previousGrounded != newGrounded)
		{
			if (newGrounded)
			{
				SetGrounded();
			}
			// Set midfly - the player started jumping
			else if (_velocityY > 0f)
			{
				SetMidfly();
			}
			// Give player a time to react by setting the Coyote timer
			else if (_coyoteTimer <= 0f)
			{
				_coyoteTimer = _coyoteTime;
			}
		}
		// Check the Coyote time
		if (_coyoteTimer > 0f)
		{
			_coyoteTimer -= Time.deltaTime;
			if (_coyoteTimer <= 0f)
			{
				SetMidfly();
			}
		}
	}

	private void Update()
	{
		CheckIfGrounded();

		if (_freezeCount > 0) return;

		currentSpeed = _speed * axis.magnitude;
		if (!Mathf.Approximately(currentSpeed, 0f))
		{
			currentSpeed = Mathf.Max(currentSpeed, _minSpeed);
		}

		float dSpeed = currentSpeed * Time.deltaTime;
		Vector3 velocity = _camera.AxisToDirection(axis) * dSpeed;

		// Applying an acceleration directly can make objects
		// fall slower at higher framerates:
		// https://discussions.unity.com/t/question-concerning-time-deltatime/122096
		if (!Mathf.Approximately(_velocityY, 0f) || !_isGrounded)
		{
			float halfGravity = _gravity * Time.deltaTime * 0.5f;

			_velocityY -= halfGravity;
			velocity.y = _velocityY * Time.deltaTime;
			_velocityY -= halfGravity;
		}
		else
		{
			// Constant downforce to prevent player standing on air
			velocity.y = -_idleDownforce * Time.deltaTime;
		}
		_velocityY = Mathf.Min(_velocityY, _maxVelocityY);

		globalVelocity = transform.TransformDirection(velocity);
		var collisionFlags = _controller.Move(globalVelocity);

		// Reset gravity if collides something above
		if ((collisionFlags & CollisionFlags.CollidedAbove) != 0 && _velocityY > 0f)
		{
			_velocityY = 0f;
		}
	}

	/// <summary>
	/// Sets the y-axis velocity, making the player jump or fall.
	/// </summary>
	/// <param name="velocity">
	/// A y-axis velocity used for the jump. Can be negative.
	/// </param>
	public void Jump(float velocity)
	{
		_velocityY = velocity;
	}

	/// <summary>
	/// Calls the <see cref="grounded" /> event unconditionally.
	/// Can be used to reset jump and dash constraints.
	/// </summary>
	public void FakeGrounded()
	{
		grounded?.Invoke(this);
	}

	/// <summary>
	/// Freezes the movement.
	/// </summary>
	public void Freeze()
	{
		_freezeCount++;
		if (_freezeCount == 1)
		{
			_velocityY = 0f;
		}
		freezeSet?.Invoke(this);
	}

	/// <summary>
	/// Unfreezes the movement.
	/// </summary>
	public void Unfreeze()
	{
		_freezeCount--;

		if (_freezeCount < 0)
		{
			_freezeCount = 0;
			Debug.Log($"Unexpected {nameof(Unfreeze)} call.");
		}
	}

	/// <summary>
	/// Unfreezes the movement completely.
	/// </summary>
	public void UnfreezeCompletely()
	{
		_freezeCount = 0;
	}
}
