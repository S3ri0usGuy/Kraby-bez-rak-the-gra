using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class DelayedAction : MonoBehaviour
{
	private float _timeLeft = -1f;

	[SerializeField, Min(0f)]
	[Tooltip("Delay in seconds.")]
	private float _delay = 5f;

	[SerializeField]
	private UnityEvent _action;

	/// <summary>
	/// Gets a time left for the action to be activated.
	/// </summary>
	/// <remarks>
	/// 0 or negative when the action has not been started.
	/// </remarks>
	public float timeLeft => _timeLeft;

	private void Update()
	{
		if (_timeLeft > 0f)
		{
			_timeLeft -= Time.deltaTime;
			if (_timeLeft <= 0f)
			{
				_timeLeft = -1f;
				_action?.Invoke();
			}
		}
	}

	public void Perform()
	{
		Perform(_delay);
	}

	public void Perform(float delay)
	{
		if (delay <= 0f)
		{
			throw new System.ArgumentOutOfRangeException(nameof(delay), "Delay must be a positive number.");
		}
		if (_timeLeft > 0f)
		{
			Debug.LogWarning($"The delayed action \"{name}\" was attempted" +
				$" to be started when it is already waiting.", gameObject);
			return;
		}
		_timeLeft = delay;
	}
}
