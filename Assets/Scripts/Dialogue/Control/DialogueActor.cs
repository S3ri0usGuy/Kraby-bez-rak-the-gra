using UnityEngine;

/// <summary>
/// A component that represents an object that gives dialogues (usually an NPC).
/// </summary>
/// <remarks>
/// It's responsible for preserving the next node of the dialogue and for
/// the dialogue's initiation. Also, it allows additional components
/// to perform custom behaviour on dialogue events.
/// </remarks>
[DisallowMultipleComponent]
public class DialogueActor : MonoBehaviour, IDialogueListener
{
	// A node that will be played next
	private DialogueNode _nextNode;

	[SerializeField]
	[Tooltip("A first node of the dialogue.")]
	private DialogueNode _firstNode;

	[SerializeField]
	private DialoguePlayer _dialoguePlayer;

	/// <summary>
	/// Gets a node that will be played when initiating the dialogue.
	/// </summary>
	public DialogueNode nextNode => _nextNode;

	/// <summary>
	/// Gets the dialogue player that is used by this component.
	/// </summary>
	public DialoguePlayer dialoguePlayer => _dialoguePlayer;

	public delegate void PhraseStartedHandler(DialoguePhraseContext context);
	public delegate void PhraseEndedHandler(DialoguePhraseContext context);
	public delegate void OptionChosenHandler(DialogueOptionContext context);
	public delegate void NodeEndedHandler(DialogueNode node, DialogueNode nextNode);
	public delegate void DialogueEndedHandler(DialogueNode node);

	/// <summary>
	/// An event that is triggered when the phrase has started.
	/// </summary>
	public event PhraseStartedHandler phraseStarted;
	/// <summary>
	/// An event that is triggered when the phrase has ended.
	/// </summary>
	public event PhraseEndedHandler phraseEnded;
	/// <summary>
	/// An event that is triggered when any dialogue option is chosen.
	/// </summary>
	public event OptionChosenHandler optionChosen;
	/// <summary>
	/// An event that is triggered when any dialogue node has ended playing.
	/// </summary>
	public event NodeEndedHandler nodeEnded;
	/// <summary>
	/// An event that is triggered when an entire dialogue ended (the player
	/// has exited the dialogue state).
	/// </summary>
	public event DialogueEndedHandler dialogueEnded;
	/// <summary>
	/// An event that is called when the <see cref="nextNode" /> changes.
	/// </summary>
	/// <remarks>
	/// This event is triggered only after an entire dialogue ends or
	/// after the <see cref="SetNextNode(DialogueNode)" /> call. It is not
	/// triggered when the node changes after answering.
	/// </remarks>
	public event DialogueEndedHandler nextNodeChanged;

	private void Start()
	{
		if (!_nextNode) _nextNode = _firstNode;
	}

	/// <summary>
	/// Sets the next node to a specific value.
	/// </summary>
	/// <param name="node">The node to be played next.</param>
	/// <exception cref="System.ArgumentNullException" />
	public void SetNextNode(DialogueNode node)
	{
		if (!node)
			throw new System.ArgumentNullException(nameof(node));

		_nextNode = node;
		nextNodeChanged?.Invoke(node);
	}

	/// <summary>
	/// Initiates the dialogue.
	/// </summary>
	public void InitiateDialogue()
	{
		if (_dialoguePlayer.isPlaying)
		{
			Debug.LogWarning("Initiate was called more than once.", gameObject);
			return;
		}

		_dialoguePlayer.Play(_nextNode, this);
	}

	void IDialogueListener.OnPhraseStarted(DialoguePhraseContext context)
	{
		phraseStarted?.Invoke(context);
	}

	void IDialogueListener.OnPhraseEnded(DialoguePhraseContext context)
	{
		phraseEnded?.Invoke(context);
	}

	void IDialogueListener.OnOptionChosen(DialogueOptionContext context)
	{
		optionChosen?.Invoke(context);
	}

	void IDialogueListener.OnNodeEnded(DialogueNode node, DialogueNode nextNode)
	{
		_nextNode = nextNode;
		nodeEnded?.Invoke(node, nextNode);
	}

	void IDialogueListener.OnEnded(DialogueNode node)
	{
		dialogueEnded?.Invoke(node);
		nextNodeChanged?.Invoke(_nextNode);
	}
}
