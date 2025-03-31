using UnityEngine;

/// <summary>
/// An object that rotates itself depending on the clock minutes left.
/// </summary>
public class Sun : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The angle which is set from the start.")]
	private Vector3 _fromAngle;
	[SerializeField]
	[Tooltip("The angle which is set when 0 minutes are left.")]
	private Vector3 _toAngle;

	private void Start()
	{
		if (Clock.exists)
		{
			UpdateRotation();
			Clock.instance.timeUpdated += OnTimeUpdated;
		}
		else
		{
			Debug.LogWarning("There is no Clock on the scene. The sun will not be active.");
		}
	}

	private void OnTimeUpdated(Clock clock)
	{
		UpdateRotation();
	}

	private void UpdateRotation()
	{
		float t = 1f - ((float)Clock.instance.minutesLeft / Clock.instance.minutesAtStart);
		transform.eulerAngles = Vector3.Slerp(_fromAngle, _toAngle, t);
	}
}
