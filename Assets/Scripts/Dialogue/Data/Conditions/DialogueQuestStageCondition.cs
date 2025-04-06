using UnityEngine;

/// <summary>
/// A condition that checks whether the quest stage has the right state.
/// </summary>
[CreateAssetMenu(fileName = "QuestStageCondition", menuName = "Dialogue/Conditions/Quest/Quest Stage")]
public class DialogueQuestStageCondition : DialogueCondition
{
	[SerializeField]
	private QuestStage _questStage;
	[SerializeField]
	private QuestStageState _state;

	public override bool IsSatisfiedFor(DialogueNode node, DialogueBranching branching)
	{
		if (!QuestSystem.exists)
		{
			Debug.LogWarning("There is no quest system present on the scene.", this);
			return false;
		}

		return QuestSystem.instance.GetStageState(_questStage) == _state;
	}
}
