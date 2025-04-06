using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that triggers the event when all of the specified triggers 
/// are triggered.
/// </summary>
public class ChainedTrigger : Trigger
{
	private bool _wasTriggered;
	private readonly HashSet<Trigger> _activatedTriggers = new();

	[SerializeField]
	private Trigger[] _triggers;

	[SerializeField]
	private UnityEvent _triggered;

	private void Awake()
	{
		foreach (var trigger in _triggers)
		{
			trigger.triggered += OnTriggered;
		}
		_wasTriggered = false;
	}

	private void OnTriggered(Trigger trigger)
	{
		if (_wasTriggered) return;

		_activatedTriggers.Add(trigger);
		if (_activatedTriggers.Count == _triggers.Length)
		{
			_wasTriggered = true;

			_triggered.Invoke();
			InvokeTriggered();
		}
	}
}
