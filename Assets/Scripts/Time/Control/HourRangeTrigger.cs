using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that triggers the event when the time left is in
/// a specific range.
/// </summary>
public class HourRangeTrigger : Trigger
{
	private enum TriggerWhen
	{
		InRange, OutRange
	}

	private bool _wasTriggered = false;

	[SerializeField, Min(0f)]
	private float _from = 7f;
	[SerializeField, Min(0f)]
	private float _to = 8.5f;
	[SerializeField]
	private TriggerWhen _triggerWhen;
	[SerializeField]
	private UnityEvent _triggered;

	private void Start()
	{
		if (!Clock.exists) return;

		Clock.instance.timeUpdated += OnTimeUpdated;

		CheckCondition();
	}

	private void OnTimeUpdated(Clock clock)
	{
		CheckCondition();
	}

	private void CheckCondition()
	{
		if (_wasTriggered) return;

		Clock clock = Clock.instance;
		int currentMinutes = clock.minutesAtStart - clock.minutesLeft + clock.startTimeMinutes;
		float currentHour = currentMinutes / 60f;

		bool condition = currentHour >= _from && currentHour <= _to;
		if (_triggerWhen == TriggerWhen.OutRange)
			condition = !condition;

		if (condition)
		{
			_wasTriggered = true;
			_triggered.Invoke();
			InvokeTriggered();
		}
	}
}
