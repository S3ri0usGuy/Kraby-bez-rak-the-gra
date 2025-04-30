using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that triggers the event when all of the specified triggers 
/// are triggered.
/// </summary>
[DisallowMultipleComponent]
public class ChainedTrigger : Trigger
{
	private bool _wasTriggered = false;
	private readonly HashSet<Trigger> _activatedTriggers = new();

	[SerializeField]
	private Trigger[] _triggers;

	[SerializeField]
	private UnityEvent _triggered;

	/// <summary>
	/// Gets/sets a flag indicating whether this component was triggered.
	/// </summary>
	public bool wasTriggered => _wasTriggered;

	private void Awake()
	{
		foreach (var trigger in _triggers)
		{
			trigger.triggered += OnTriggered;
		}
	}

	private void Start()
	{
#if UNITY_EDITOR
		if (SaveSystem.exists && !TryGetComponent<ChainedTriggerSaver>(out _))
		{
			Debug.LogWarning($"The chained trigger \"{name}\" has" +
				$" no {nameof(ChainedTriggerSaver)} component assigned to it." +
				$" Without it, the chained trigger may not work properly with the save system.",
				gameObject);
		}
#endif
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

	/// <summary>
	/// Deactivates the trigger by gaslighting it into believing that
	/// it has already been triggered.
	/// </summary>
	public void Deactivate()
	{
		_wasTriggered = true;
	}
}
