using UnityEngine;

/// <summary>
/// Represents a condition of the dialogue node.
/// </summary>
public abstract class DialogueCondition : ScriptableObject
{
	/// <summary>
	/// Checks whether the condition is satisfied for the specified
	/// <paramref name="node"/> and <paramref name="actor"/>.
	/// </summary>
	/// <param name="node">A node to check against.</param>
	/// <param name="branching">A branching hat contains this condition.</param>
	/// <param name="actor">An actor that is related to the node.</param>
	/// <returns>
	/// <see langword="true" /> if this condition is satisfied;
	/// otherwise <see langword="false" />.
	/// </returns>
	public abstract bool IsSatisfiedFor(DialogueNode node, DialogueBranching branching, DialogueActor actor);
}
