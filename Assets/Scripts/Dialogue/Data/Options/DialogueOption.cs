using System;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Represents a dialogue option that the player can choose.
/// </summary>
[Serializable]
public class DialogueOption
{
	[SerializeField]
	[Tooltip("A short text description of the option. " +
		"May differ from the actual phrase.")]
	private LocalizedString _text;
	[SerializeField]
	[Tooltip("Parameters that decide to which node this option leads. " +
		"If no nodes are available in the branches, the option will not be visible.")]
	private DialogueBranching _branching;

	/// <summary>
	/// Gets a short localized text description of the option.
	/// </summary>
	public LocalizedString text => _text;
	/// <summary>
	/// Gets the parameters that decide to which node this option leads.
	/// If no nodes are available in the branches, the option will not be visible.
	/// </summary>
	public DialogueBranching branching => _branching;
}
