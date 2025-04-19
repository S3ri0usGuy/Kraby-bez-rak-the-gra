using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// A condition that checks whether the subquest has the right state.
/// </summary>
[CreateAssetMenu(fileName = "SubquestStateCondition", menuName = "Dialogue/Conditions/Quest/Subquest State")]
public class DialogueSubquestStateCondition : DialogueCondition
{
	[SerializeField]
	[FormerlySerializedAs("_questStage")]
	private Subquest _subquest;
	[SerializeField]
	private SubquestState _state;

	public override bool IsSatisfiedFor(DialogueNode node, DialogueBranching branching)
	{
		if (!QuestSystem.exists)
		{
			Debug.LogWarning("There is no squest system present on the scene.", this);
			return false;
		}

		return QuestSystem.instance.GetSubquestState(_subquest) == _state;
	}
}
