using UnityEngine;

/// <summary>
/// A dialogue condition that has a probability to be satisfied.
/// </summary>
[CreateAssetMenu(fileName = "DialogueRandomCondition", menuName = "Dialogue/Conditions/Random")]
public class DialogueRandomCondition : DialogueCondition
{
	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("A probability of the condition to be satisfied.")]
	private float _probability = 0.5f;

	public override bool IsSatisfiedFor(DialogueNode node, DialogueBranching branching)
	{
		return RandomUtils.Bool(_probability);
	}
}
