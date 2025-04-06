using UnityEngine;

/// <summary>
/// A component that allows to complete or fail a quest. Can be
/// used by Unity events.
/// </summary>
public class QuestAction : MonoBehaviour
{
	public enum QuestActionType
	{
		Complete,
		Fail
	}

	[SerializeField]
	[Tooltip("A quest that is the receiver of this action.")]
	private Quest _quest;
	[SerializeField]
	[Tooltip("An action to perform.")]
	private QuestActionType _action;

	public void Perform()
	{
		QuestState state = _action switch
		{
			QuestActionType.Complete => QuestState.Completed,
			QuestActionType.Fail => QuestState.Failed,
			_ => throw new System.InvalidOperationException("Invalid quest action type.")
		};

		QuestSystem.instance.SetQuestState(_quest, state);
	}
}
