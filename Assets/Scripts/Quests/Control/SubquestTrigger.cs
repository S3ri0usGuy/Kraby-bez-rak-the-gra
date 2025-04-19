using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// A component that triggers events when the subquest state is changed.
/// </summary>
public class SubquestTrigger : Trigger
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
	private UnityEvent _triggered;

	private void OnEnable()
	{
		if (QuestSystem.exists)
		{
			QuestSystem.instance.subquestStateUpdated += OnSubquestUpdated;
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

		throw new System.InvalidOperationException("");
	}

	private void OnSubquestUpdated(QuestSystem questSystem, SubquestStateUpdatedEventArgs e)
	{
		if (e.subquest == _subquest && IsTriggered(e.newSubquestState))
		{
			_triggered.Invoke();
			InvokeTriggered();
		}
	}
}
