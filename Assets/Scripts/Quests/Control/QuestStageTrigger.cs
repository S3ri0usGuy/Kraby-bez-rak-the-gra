using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that triggers events when the quest stage is changed.
/// </summary>
public class QuestStageTrigger : MonoBehaviour
{
	public enum TriggerType
	{
		StateChanged,
		Started,
		Completed,
		Failed
	}

	[SerializeField]
	private QuestStage _stage;
	[SerializeField]
	[Tooltip("An option that defines when the event is triggered.")]
	private TriggerType _triggerType;

	[SerializeField]
	private UnityEvent _triggered;

	private void OnEnable()
	{
		if (QuestSystem.exists)
		{
			QuestSystem.instance.questStageUpdated += OnStageUpdated;
		}
	}

	private bool IsTriggered(QuestStageState state)
	{
		switch (_triggerType)
		{
			case TriggerType.StateChanged:
				return true;
			case TriggerType.Started:
				return state == QuestStageState.Active;
			case TriggerType.Completed:
				return state == QuestStageState.Completed;
			case TriggerType.Failed:
				return state == QuestStageState.Failed;
		}

		throw new System.InvalidOperationException("");
	}

	private void OnStageUpdated(QuestSystem questSystem, QuestStageUpdatedEventArgs e)
	{
		if (e.stage == _stage && IsTriggered(e.newStageState))
		{
			_triggered.Invoke();
		}
	}
}
