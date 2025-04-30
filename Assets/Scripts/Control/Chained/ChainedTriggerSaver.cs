using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ChainedTrigger))]
public class ChainedTriggerSaver : SavableComponent<ChainedTriggerSaveData>
{
	private ChainedTrigger _chainedTrigger;

	protected override ChainedTriggerSaveData fallbackData => new() { wasTriggered = false };

	protected override void Awake()
	{
		_chainedTrigger = GetComponent<ChainedTrigger>();
		base.Awake();
	}

	protected override void OnLoad()
	{
		if (data.wasTriggered)
		{
			_chainedTrigger.Deactivate();
		}
	}

	protected override void OnSave()
	{
		data.wasTriggered = _chainedTrigger.wasTriggered;
	}
}
