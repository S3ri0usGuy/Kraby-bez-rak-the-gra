using UnityEngine;

/// <summary>
/// A condition that checks whether the quest has the right state.
/// </summary>
[CreateAssetMenu(fileName = "DialogueQuestStateCondition", menuName = "Dialogue/Conditions/Quest/Quest State")]
public class DialogueQuestStateCondition : DialogueCondition
{
	[SerializeField]
	private Quest _quest;
	[SerializeField]
	private QuestState _state;

	public override bool IsSatisfiedFor(DialogueNode node, DialogueBranching branching)
	{
		if (!QuestSystem.exists)
		{
			Debug.LogWarning("There is no quest system present on the scene.", this);
			return false;
		}

		return QuestSystem.instance.GetQuestState(_quest) == _state;
	}
}
