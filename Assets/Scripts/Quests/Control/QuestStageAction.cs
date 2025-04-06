using UnityEngine;

/// <summary>
/// A component that allows to complete or fail a the quest stage. Can be
/// used by Unity events.
/// </summary>
public class QuestStageAction : MonoBehaviour
{
	public enum QuestStageActionType
	{
		Reveal,
		Complete,
		Fail
	}

	[SerializeField]
	private bool _performOnEnable = false;
	[SerializeField]
	[Tooltip("A quest that is the receiver of this action.")]
	private QuestStage _stage;
	[SerializeField]
	[Tooltip("An action to perform.")]
	private QuestStageActionType _action;

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
			case QuestStageActionType.Reveal:
				if (questSystem.GetStageState(_stage) == QuestStageState.None)
				{
					questSystem.SetStageState(_stage, QuestStageState.Active);
				}
				break;

			case QuestStageActionType.Complete:
				questSystem.SetStageState(_stage, QuestStageState.Completed);
				break;

			case QuestStageActionType.Fail:
				questSystem.SetStageState(_stage, QuestStageState.Failed);
				break;
		}
	}
}
