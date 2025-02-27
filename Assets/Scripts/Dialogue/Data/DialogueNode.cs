using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An asset that represents the dialogue node. Stores phrases
/// and branches to other nodes.
/// </summary>
[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
	/// <summary>
	/// A limit of the dialogue answer options.
	/// </summary>
	public const int maxDialogueOptions = 4;

	[Header("Phrases")]
	[SerializeField]
	[Tooltip("An ordered collection of phrases in the node.")]
	private DialoguePhrase[] _phrases;

	[Header("Answering")]
	[SerializeField]
	[Tooltip("Options that are given to the player after the last phrase. " +
		"If empty, the dialogue will end.")]
	private DialogueOption[] _options;
	[SerializeField]
	private DialogueAnswerParams _answerParams;

	[Header("Branching")]
	[SerializeField]
	[Tooltip("A next node that is set as current after this started playing. " +
		"If this node has options, this parameter will probably not do anything. " +
		"Leave empty to set this node as the current.")]
	private DialogueNode _nextNode;

	/// <summary>
	/// Gets an ordered collection of phrases in the node.
	/// </summary>
	public IReadOnlyList<DialoguePhrase> phrases => _phrases;
	/// <summary>
	/// Gets an ordered collection of options that are given to the player
	/// after the last phrase. If empty, the dialogue will end.
	/// </summary>
	public IReadOnlyList<DialogueOption> options => _options;
	/// <summary>
	/// Get the answer parameters that are used for this node's options.
	/// </summary>
	public DialogueAnswerParams answerParams => _answerParams;

	/// <summary>
	/// Gets a next node that is set as current after this started playing.
	/// If this node has options, this parameter will probably not do anything. 
	/// If <see langword="null"/>, this node will be set as the current."
	/// </summary>
	public DialogueNode nextNode => _nextNode;
}
