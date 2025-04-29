using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// A component that triggers events when the subquest state is changed.
/// </summary>
public class SubquestStateTrigger : Trigger
{
	public enum TriggerType
	{
		StateChanged,
		Started,
		Completed,
		Failed
	}

	[SerializeField]
	[FormerlySerializedAs("_stage")]
	private Subquest _subquest;
	[SerializeField]
	[Tooltip("An option that defines when the event is triggered.")]
	private TriggerType _triggerType;
	[SerializeField]
	[FormerlySerializedAs("_checkOnStart")]
	[Tooltip("If checked, the state will be checked after loading the game. " +
		"Use this if you want to check the state after continuing the game.")]
	private bool _initialStateCheck = true;

	[SerializeField]
	private UnityEvent _triggered;

	private void Start()
	{
		if (!QuestSystem.exists)
		{
			Debug.LogWarning($"There is no quest system on the scene, the " +
				$"trigger \"{name}\" will not work.", gameObject);
			return;
		}
		QuestSystem questSystem = QuestSystem.instance;

		questSystem.subquestStateUpdated += OnSubquestUpdated;
		if (_initialStateCheck && IsTriggered(questSystem.GetSubquestState(_subquest)))
		{
			Trigger();
		}
	}

	private bool IsTriggered(SubquestState state)
	{
		switch (_triggerType)
		{
			case TriggerType.StateChanged:
				return true;
			case TriggerType.Started:
				return state == SubquestState.Active;
			case TriggerType.Completed:
				return state == SubquestState.Completed;
			case TriggerType.Failed:
				return state == SubquestState.Failed;
		}

		throw new System.InvalidOperationException("Invalid subquest state.");
	}

	private void OnSubquestUpdated(QuestSystem questSystem, SubquestStateUpdatedEventArgs e)
	{
		if (e.subquest == _subquest && IsTriggered(e.newSubquestState))
		{
			Trigger();
		}
	}

	private void Trigger()
	{
		_triggered.Invoke();
		InvokeTriggered();
	}
}
