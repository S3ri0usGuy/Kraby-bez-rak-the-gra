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
	[Tooltip("A branching that decides which node will be set as current after this node. " +
		"If this node has at least one availble option, this parameter will be ignored. " +
		"If branching fails, this node will remain the current.")]
	private DialogueBranching _branching;

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
	/// A branching that decides which node will be set as current after this node.
	/// If this node has at least one availble option, this parameter will be ignored.
	/// If branching returns <see langword="null"/>, this node will remain the current.
	/// </summary>
	public DialogueBranching branching => _branching;
}
