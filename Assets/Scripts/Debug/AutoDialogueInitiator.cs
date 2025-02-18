using UnityEngine;

/// <summary>
/// A component that automatically initiates a dialogue on enable.
/// </summary>
[RequireComponent(typeof(DialogueActor))]
public class AutoDialogueInitiator : MonoBehaviour
{
	private DialogueActor _dialogueActor;

	private void Awake()
	{
		_dialogueActor = GetComponent<DialogueActor>();
	}

	private void OnEnable()
	{
		_dialogueActor.InitiateDialogue();
	}
}
