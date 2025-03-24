using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// A component that sets the x axis recentering center.
/// </summary>
public class CameraAutoXRecenter : MonoBehaviour
{
	[SerializeField]
	private CinemachineOrbitalFollow _orbitalFollow;
	[SerializeField]
	[Tooltip("The target that this component follows.")]
	private Transform _target;
	[SerializeField]
	private PlayerMovement _movement;
	[SerializeField]
	[Tooltip("The y axis threshold when this component starts recentering.")]
	private float _yAxisThreshold = -0.1f;

	private void LateUpdate()
	{
		if (_movement.axis.y > _yAxisThreshold)
		{
			_orbitalFollow.HorizontalAxis.Center = _target.eulerAngles.y;
		}
	}
}
