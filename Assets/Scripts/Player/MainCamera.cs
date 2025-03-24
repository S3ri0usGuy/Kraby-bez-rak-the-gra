using Unity.Cinemachine;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
	private bool _needsRefresh = true;
	private Vector3 _forward;
	private Vector3 _right;

	[SerializeField]
	private CinemachineCamera _freeLook;
	[SerializeField]
	private Transform _target;

	/// <summary>
	/// Gets a forward direction of the camera on a XZ plane.
	/// </summary>
	/// <remarks>
	/// Is not guaranteed to work in late update.
	/// </remarks>
	public Vector3 forward
	{
		get
		{
			Refresh();
			return _forward;
		}
	}
	/// <summary>
	/// Gets a forward direction of the camera on a XZ plane.
	/// </summary>
	/// <remarks>
	/// Is not guaranteed to work in late update.
	/// </remarks>
	public Vector3 right
	{
		get
		{
			Refresh();
			return _right;
		}
	}

	public CinemachineCamera freeLook => _freeLook;

	private void OnEnable()
	{
		StartLooking();
	}

	private void LateUpdate()
	{
		_needsRefresh = true;
	}

	private void Refresh()
	{
		if (!_needsRefresh) return;

		// Ignore x and z rotation
		Quaternion cameraRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
		_forward = cameraRotation * Vector3.forward;
		_right = cameraRotation * Vector3.right;

		_needsRefresh = false;
	}

	public Vector3 AxisToDirection(Vector2 axis)
	{
		Refresh();
		return (_forward * axis.y + _right * axis.x).normalized;
	}

	public void StartLooking()
	{
		freeLook.LookAt = freeLook.Follow = _target;
	}

	public void StopLooking()
	{
		freeLook.LookAt = freeLook.Follow = null;
	}

	public void ResetTransform()
	{
		_freeLook.PreviousStateIsValid = false;
	}
}
