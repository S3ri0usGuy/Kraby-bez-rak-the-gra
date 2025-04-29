using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that triggers events when the quest state is changed.
/// </summary>
public class QuestStateTrigger : Trigger
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
	[Tooltip("If toggled, the state will be checked after loading the game. " +
		"Use this if you want to check the state after continuing the game.")]
	private bool _checkOnStart = true;

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

		questSystem.questStateUpdated += OnQuestUpdated;
		if (_checkOnStart && IsTriggered(questSystem.GetQuestState(_quest)))
		{
			Trigger();
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
			Trigger();
		}
	}

	private void Trigger()
	{
		_triggered.Invoke();
		InvokeTriggered();
	}
}
