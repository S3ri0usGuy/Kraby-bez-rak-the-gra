using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that auto plays the dialogue until it ends on the same node twice.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(DialogueActor))]
public class DialogueAutoPlayer : MonoBehaviour
{
	private DialogueActor _actor;
	private DialogueNode _previousNode;

	[SerializeField]
	[Tooltip("An event that is triggered when the auto player stopped.")]
	private UnityEvent _ended;

	private void Awake()
	{
		_actor = GetComponent<DialogueActor>();
		_actor.dialoguePlayer.ended.AddListener(OnDialogueEnded);
	}

	private void OnDialogueEnded()
	{
		if (_actor.nextNode == _previousNode)
		{
			_ended.Invoke();
			return;
		}

		_previousNode = _actor.nextNode;
		_actor.InitiateDialogue();
	}
}
