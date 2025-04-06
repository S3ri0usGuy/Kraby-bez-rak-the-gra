using UnityEngine;

/// <summary>
/// A component that allows to complete or fail a the quest stage. Can be
/// used by Unity events.
/// </summary>
public class QuestStageAction : MonoBehaviour
{
	public enum QuestStageActionType
	{
		Complete,
		Fail
	}

	[SerializeField]
	[Tooltip("A quest that is the receiver of this action.")]
	private QuestStage _stage;
	[SerializeField]
	[Tooltip("An action to perform.")]
	private QuestStageActionType _action;

	public void Perform()
	{
		QuestStageState state = _action switch
		{
			QuestStageActionType.Complete => QuestStageState.Completed,
			QuestStageActionType.Fail => QuestStageState.Failed,
			_ => throw new System.InvalidOperationException("Invalid quest stage action type.")
		};

		QuestSystem.instance.SetStageState(_stage, state);
	}
}
