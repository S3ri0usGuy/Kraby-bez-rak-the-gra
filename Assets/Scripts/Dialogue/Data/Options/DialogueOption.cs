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
	[Tooltip("The next dialogue node that this option leads to.")]
	private DialogueNode _nextNode;

	/// <summary>
	/// Gets a short localized text description of the option.
	/// </summary>
	public LocalizedString text => _text;
	/// <summary>
	/// Gets the next dialogue node that this option leads to.
	/// </summary>
	public DialogueNode nextNode => _nextNode;
}
