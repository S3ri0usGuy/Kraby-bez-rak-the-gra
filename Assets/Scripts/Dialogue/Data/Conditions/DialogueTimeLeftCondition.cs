using UnityEngine;

/// <summary>
/// A dialogue condition that is based on the time left.
/// </summary>
[CreateAssetMenu(fileName = "DialogueTimeLeftCondition", menuName = "Dialogue/Conditions/Time Left")]
public class DialogueTimeLeftCondition : DialogueCondition
{
	[SerializeField]
	[Tooltip("A comparison to use.")]
	private Comparison _comparison;
	[SerializeField, Min(0)]
	[Tooltip("A value to compare the actual minutes left to.")]
	private int _targetMinutesLeft;

	public override bool IsSatisfiedFor(DialogueNode node, DialogueBranching branching)
	{
		if (!Clock.exists)
		{
			Debug.LogWarning($"The Clock doesn't exists. The condition {name} was ignored.");
			return true;
		}

		return _comparison.Compare(Clock.instance.minutesLeft, _targetMinutesLeft);
	}
}
