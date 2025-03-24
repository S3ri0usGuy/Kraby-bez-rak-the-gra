using UnityEngine;

/// <summary>
/// A component that manages the player's rotation.
/// </summary>
public class PlayerRotation : MonoBehaviour
{
	private Quaternion _targetRotation;

	private PlayerMovement _movement;
	private float _forceDuration = 0f;

	[SerializeField, Min(0f)]
	private float _angularSpeed = 270f;

	private void Awake()
	{
		_movement = this.TryGetInParent<PlayerMovement>();
	}

	private void OnEnable()
	{
		_forceDuration = 0f;
	}

	private void Update()
	{
		Vector2 axis = _movement.axis;
		if (!Mathf.Approximately(axis.sqrMagnitude, 0f))
		{
			Vector3 direction = _movement.mainCamera.AxisToDirection(axis);
			_targetRotation = Quaternion.LookRotation(
				direction, transform.up);
		}
	}

	private void LateUpdate()
	{
		if (_forceDuration > 0f)
		{
			_forceDuration -= Time.deltaTime;
			return;
		}
		if (_movement.isFreezed)
			return;

		transform.rotation = Quaternion.RotateTowards(
			transform.rotation, _targetRotation, _angularSpeed * Time.deltaTime);
	}

	/// <summary>
	/// Sets the player rotation.
	/// </summary>
	/// <param name="rotation">A new rotation.</param>
	/// <param name="duration">For how long the new rotation must be forced.</param>
	public void ForceRotation(Quaternion rotation, float duration = 0f)
	{
		transform.rotation = rotation;

		_forceDuration = duration;
	}

	/// <summary>
	/// Sets the player rotation to a specified direction.
	/// </summary>
	/// <param name="forward">A new forward direction.</param>
	/// <param name="duration">For how long the new rotation must be forced.</param>
	public void ForceRotation(Vector3 forward, float duration = 0f)
	{
		forward.y = 0f;
		ForceRotation(Quaternion.LookRotation(forward, transform.up), duration);
	}
}
