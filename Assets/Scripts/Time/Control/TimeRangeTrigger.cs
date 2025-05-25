using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that triggers the event when the time left is in
/// a specific range.
/// </summary>
[System.Obsolete("Use HourRangeTrigger instead.")]
public class TimeRangeTrigger : Trigger
{
	private bool _wasTriggered = false;

	[SerializeField, Min(0)]
	private int _minMinutesLeft;
	[SerializeField, Min(0)]
	private int _maxMinutesLeft;
	[SerializeField]
	private UnityEvent _triggered;

	private void Start()
	{
		if (!Clock.exists) return;

		Clock.instance.timeUpdated += OnTimeUpdated;

		CheckCondition();
	}

	private void OnValidate()
	{
		_maxMinutesLeft = Mathf.Max(_maxMinutesLeft, _minMinutesLeft);
	}

	private void OnTimeUpdated(Clock clock)
	{
		CheckCondition();
	}

	private void CheckCondition()
	{
		if (_wasTriggered) return;

		if (Clock.instance.minutesLeft >= _minMinutesLeft &&
			Clock.instance.minutesLeft <= _maxMinutesLeft)
		{
			_wasTriggered = true;

			_triggered.Invoke();
			InvokeTriggered();
		}
	}
}
