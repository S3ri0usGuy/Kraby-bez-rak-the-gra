using UnityEngine;

/// <summary>
/// A component that represents an object that gives dialogues (usually an NPC).
/// </summary>
/// <remarks>
/// It's responsible for preserving the next node of the dialogue and for
/// the dialogue's initiation. Also, it allows additional components
/// to perform custom behaviour on dialogue events (will be implemented in future).
/// </remarks>
public class DialogueActor : MonoBehaviour, IDialogueListener
{
	// TODO: integrate with the save system
	// TODO: add dialogue events

	// A node that will be played next
	private DialogueNode _nextNode;

	[SerializeField]
	[Tooltip("A first node of the dialogue.")]
	private DialogueNode _firstNode;

	[SerializeField]
	private DialoguePlayer _dialoguePlayer;

	private void Start()
	{
		_nextNode = _firstNode;
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
