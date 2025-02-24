using UnityEngine;

/// <summary>
/// A component that represents an object that gives dialogues (usually an NPC).
/// </summary>
/// <remarks>
/// It's responsible for preserving the next node of the dialogue and for
/// the dialogue's initiation. Also, it allows additional components
/// to perform custom behaviour on dialogue events (will be implemented in future).
/// </remarks>
[DisallowMultipleComponent]
public class DialogueActor : MonoBehaviour, IDialogueListener
{
	// TODO: add dialogue events

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

	}

	void IDialogueListener.OnPhraseEnded(DialoguePhraseContext context)
	{

	}

	void IDialogueListener.OnOptionChosen(DialogueOptionContext context)
	{

	}

	void IDialogueListener.OnNodeEnded(DialogueNode node, DialogueNode nextNode)
	{
		_nextNode = nextNode;
	}

	void IDialogueListener.OnEnded(DialogueNode node)
	{

	}
}
