using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that triggers events when the quest state is changed.
/// </summary>
public class QuestStateTrigger : MonoBehaviour
{
	public enum TriggerType
	{
		StateChanged,
		Started,
		Completed,
		Failed
	}

	[SerializeField]
	private Quest _quest;
	[SerializeField]
	[Tooltip("An option that defines when the event is triggered.")]
	private TriggerType _triggerType;

	[SerializeField]
	private UnityEvent _triggered;

	private void OnEnable()
	{
		if (QuestSystem.exists)
		{
			QuestSystem.instance.questStateUpdated += OnQuestUpdated;
		}
	}

	private bool IsTriggered(QuestState state)
	{
		switch (_triggerType)
		{
			case TriggerType.StateChanged:
				return true;
			case TriggerType.Started:
				return state == QuestState.Active;
			case TriggerType.Completed:
				return state == QuestState.Completed;
			case TriggerType.Failed:
				return state == QuestState.Failed;
		}

		throw new System.InvalidOperationException("");
	}

	private void OnQuestUpdated(QuestSystem questSystem, QuestStateUpdatedEventArgs e)
	{
		if (_quest == e.quest && IsTriggered(e.newQuestState))
		{
			_triggered.Invoke();
		}
	}
}
