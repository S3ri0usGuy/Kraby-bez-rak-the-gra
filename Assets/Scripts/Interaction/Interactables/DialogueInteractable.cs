using UnityEngine;

public class DialogueInteractable : Interactable
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

	public override bool CanBeInteractedWith(Player player)
	{
		return true;
	}

	private void OnDialogueEnded(DialogueNode node)
	{
		_timer = _cooldown;
	}

	public override bool Interact(Player player)
	{
		if (_timer > 0f) return false;

		_dialogueActor.InitiateDialogue();
		return false;
	}
}
