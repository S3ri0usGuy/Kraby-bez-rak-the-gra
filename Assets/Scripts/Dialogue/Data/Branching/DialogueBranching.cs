using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents dialogue branching parameters.
/// </summary>
[Serializable]
public class DialogueBranching
{
	/// <summary>
	/// Represents a dialogue branch.
	/// </summary>
	[Serializable]
	public class DialogueBranch
	{
		[SerializeField]
		[Tooltip("A node assigned to the branch.")]
		private DialogueNode _node;
		[SerializeField]
		[Tooltip("A list of conditions that must be satisfied in order for the " +
			"node to be available.")]
		private DialogueCondition[] _conditions;

		/// <summary>
		/// Gets a node assigned to the branch.
		/// </summary>
		public DialogueNode node => _node;
		/// <summary>
		/// Gets a readonly collection of conditions that must be satisfied
		/// in order for the node to be available.
		/// </summary>
		public IReadOnlyList<DialogueCondition> conditions => _conditions;
	}

	[SerializeField]
	[Tooltip("The next dialogue node that this option leads to if there " +
		"are no available alternatives. Can be left empty.")]
	private DialogueNode _defaultNextNode;

	[SerializeField]
	[Tooltip("A collection of all possible branches. Only the first one " +
		"with all satisfied conditions is selected.")]
	private DialogueBranch[] _branches;

	/// <summary>
	/// Gets the next dialogue node that this option leads to if there
	/// are no available alternatives. Can be <see langword="null" />.
	/// </summary>
	public DialogueNode defaultNextNode => _defaultNextNode;

	/// <summary>
	/// Gets a readonly collection of all possible branches. Only the first one
	/// with all satisfied conditions is selected.
	/// </summary>
	public IReadOnlyList<DialogueBranch> branches => _branches;

	/// <summary>
	/// Selects the first available node.
	/// </summary>
	/// <returns>
	/// The first available dialogue node from the <see cref="branches" /> or
	/// the <see cref="defaultNextNode" />. Can be <see langword="null" />.
	/// </returns>
	public DialogueNode SelectNode()
	{
		foreach (var branch in branches)
		{
			if (!branch.node)
			{
				Debug.LogWarning("A branch without valid node was detected.");
				continue;
			}

			if (branch.conditions.All(x => x.IsSatisfiedFor(branch.node, this)))
			{
				return branch.node;
			}
		}

		return _defaultNextNode;
	}
}
