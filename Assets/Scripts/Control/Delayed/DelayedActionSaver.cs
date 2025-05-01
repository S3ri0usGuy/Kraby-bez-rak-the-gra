using UnityEngine;

/// <summary>
/// A component that saves the <see cref="DelayedAction" /> state.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(DelayedAction))]
public class DelayedActionSaver : SavableComponent<DelayedActionSaveData>
{
	private DelayedAction _delayedAction;

	protected override DelayedActionSaveData fallbackData => new() { timeLeft = -1f };

	protected override void Awake()
	{
		_delayedAction = GetComponent<DelayedAction>();
		base.Awake();
	}

	protected override void OnLoad()
	{
		if (data.timeLeft > 0f)
		{
			_delayedAction.Perform(data.timeLeft);
		}
	}

	protected override void OnSave()
	{
		data.timeLeft = _delayedAction.timeLeft;
	}
}
