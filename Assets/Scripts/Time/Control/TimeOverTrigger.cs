using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that triggers the event when the time is over.
/// </summary>
public class TimeOverTrigger : Trigger
{
	[SerializeField]
	[Tooltip("If checked, the component will be triggered if " +
		"the game has started with 0 minutes left.")]
	private bool _initialStateCheck = true;
	[SerializeField]
	private UnityEvent _triggered;

	private void Start()
	{
		if (!Clock.exists) return;

		Clock.instance.timeOver += OnTimeOver;

		if (_initialStateCheck && Clock.instance.minutesLeft <= 0)
		{
			_triggered.Invoke();
			InvokeTriggered();
		}
	}

	private void OnTimeOver(Clock clock)
	{
		_triggered.Invoke();
		InvokeTriggered();
	}
}
