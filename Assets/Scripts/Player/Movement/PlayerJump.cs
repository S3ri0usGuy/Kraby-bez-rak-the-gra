using System;
using UnityEngine;

/// <summary>
/// A component that provides the jump mechanic for the player.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class PlayerJump : MonoBehaviour
{
	private PlayerMovement _movement;

	[SerializeField]
	[Tooltip("A vertical speed of the jump.")]
	private ModifiableFloat _jumpVelocity = new(22f, minValue: 0f);

	/// <summary>
	/// Gets a modifiable vertical speed of the jump.
	/// </summary>
	public ModifiableFloat jumpVelocity => _jumpVelocity;

	/// <summary>
	/// An event that is triggered when the jump is performed.
	/// </summary>
	public event Action<PlayerJump> performed;

	private void Awake()
	{
		_movement = GetComponent<PlayerMovement>();
	}

	/// <summary>
	/// Performs a jump if it's possible.
	/// </summary>
	/// <returns>
	/// <see langword="true" /> if a jump was performed successfully;
	/// otherwise <see langword="false" />.
	/// </returns>
	public bool Perform()
	{
		if (!enabled || !gameObject.activeInHierarchy)
			return false;
		if (_movement.isFreezed || !_movement.isGrounded)
			return false;

		_movement.Jump(_jumpVelocity);
		performed?.Invoke(this);

		return true;
	}
}
