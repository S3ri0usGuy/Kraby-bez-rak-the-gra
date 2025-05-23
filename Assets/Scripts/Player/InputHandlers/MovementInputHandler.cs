using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A component that is responsible for handling the movement input.
/// </summary>
public class MovementInputHandler : MonoBehaviour
{
	private InputActions.PlayerActions _actions;

	private PlayerMovement _movement;
	private PlayerJump _jump;

	private InputBuffer _jumpBuffer;

	[SerializeField, Min(0f)]
	private float _jumpBufferWindow = InputBuffer.defaultWindow;

	private void Awake()
	{
		_actions = InputActionsProvider.RetrieveFor(this).player;
		_movement = this.TryGetInParent<PlayerMovement>();
		_jump = _movement.TryGetInChildren<PlayerJump>();

		_jumpBuffer = new(_ => _jump.Perform(), this, _jumpBufferWindow);
	}

	private void OnEnable()
	{
		_actions.move.performed += OnMovementPerformed;
		_actions.move.canceled += OnMovementCanceled;
		_actions.jump.performed += _jumpBuffer.PerformedListener;
		_actions.sprint.performed += OnSprintPerformed;
		_actions.sprint.canceled += OnSprintCanceled;

		_movement.axis = _actions.move.ReadValue<Vector2>();
		_movement.isSprinting = false;
	}

	private void OnDisable()
	{
		_actions.move.performed -= OnMovementPerformed;
		_actions.jump.performed -= _jumpBuffer.PerformedListener;
		_actions.sprint.performed -= OnSprintPerformed;
		_actions.sprint.canceled -= OnSprintCanceled;

		StopAllCoroutines();

		_movement.axis = Vector2.zero;
		_movement.isSprinting = false;
	}

	private void OnMovementPerformed(InputAction.CallbackContext context)
	{
		_movement.axis = context.ReadValue<Vector2>();
	}

	private void OnMovementCanceled(InputAction.CallbackContext obj)
	{
		_movement.axis = Vector2.zero;
	}

	private void OnSprintPerformed(InputAction.CallbackContext context)
	{
		_movement.isSprinting = true;
	}

	private void OnSprintCanceled(InputAction.CallbackContext context)
	{
		_movement.isSprinting = false;
	}
}