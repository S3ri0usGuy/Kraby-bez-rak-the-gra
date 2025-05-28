using UnityEngine;

public class DialogueInteractable : InteractableObject
{
	private float _timer;

	[SerializeField, Min(0f)]
	private float _cooldown = 0.4f;
	[SerializeField]
	private DialogueActor _dialogueActor;

	private void Start()
	{
		_dialogueActor.dialogueEnded += OnDialogueEnded;
	}

	private void Update()
	{
		if (_timer > 0f)
		{
			_timer -= Time.deltaTime;
		}
	}

	protected override bool CanBeInteractedWith(Player player)
	{
		if (_dialogueActor.dialoguePlayer.isPlaying) return false;

		var currentNode = _dialogueActor.nextNode;
		if (currentNode.phrases.Count == 0)
		{
			var nextSelectedNode = currentNode.branching.SelectNode();
			if (nextSelectedNode)
			{
				_dialogueActor.SetNextNode(nextSelectedNode);
				return true;
			}
			return false;
		}
		return true;
	}

	private void OnDialogueEnded(DialogueNode node)
	{
		_timer = _cooldown;
	}

	protected override bool PerformInteract(Player player)
	{
		if (_timer > 0f) return false;

		_dialogueActor.InitiateDialogue();
		return false;
	}
}
