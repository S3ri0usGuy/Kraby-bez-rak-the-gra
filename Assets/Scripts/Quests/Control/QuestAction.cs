using UnityEngine;

/// <summary>
/// A component that allows to complete or fail a quest. Can be
/// used by Unity events.
/// </summary>
public class QuestAction : MonoBehaviour
{
	public enum QuestActionType
	{
		Reveal,
		Complete,
		Fail
	}

	[SerializeField]
	private bool _performOnEnable = false;
	[SerializeField]
	[Tooltip("A quest that is the receiver of this action.")]
	private Quest _quest;
	[SerializeField]
	[Tooltip("An action to perform.")]
	private QuestActionType _action;

	private void OnEnable()
	{
		if (_performOnEnable)
		{
			Perform();
		}
	}

	public void Perform()
	{
		QuestSystem questSystem = QuestSystem.instance;
		switch (_action)
		{
			case QuestActionType.Reveal:
				if (questSystem.GetQuestState(_quest) == QuestState.None)
				{
					questSystem.SetQuestState(_quest, QuestState.Active);
				}
				break;

			case QuestActionType.Complete:
				questSystem.SetQuestState(_quest, QuestState.Completed);
				break;

			case QuestActionType.Fail:
				questSystem.SetQuestState(_quest, QuestState.Failed);
				break;
		}
	}
}
