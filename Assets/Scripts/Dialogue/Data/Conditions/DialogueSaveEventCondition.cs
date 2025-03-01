using UnityEngine;

/// <summary>
/// An dialogue event that checks the save event key.
/// </summary>
public class DialogueSaveEventCondition : DialogueCondition
{
	public enum DialogueEventState
	{
		Exists,
		NotExists
	}

	[SerializeField]
	[Tooltip("A target state of the event key that satisfies the condition. " +
		"'Exists' means that the event was added and 'NotExists' means that " +
		"the event was removed or not triggered.")]
	private DialogueEventState _targetState;
	[SerializeField]
	[Tooltip("An event that is checked in this condition.")]
	private DialogueSaveEvent _event;

	private DialogueEventState GetState()
	{
		return DialogueSaveEventsProvider.instance.ContainsKey(_event.id) ?
			DialogueEventState.Exists : DialogueEventState.NotExists;
	}

	public override bool IsSatisfiedFor(DialogueNode node, DialogueBranching branching)
	{
		if (!_event)
		{
			Debug.LogError($"The dialogue event condition \"{name}\" " +
				$"has no dialogue event assigned to it.", this);
			return false;
		}

		return GetState() == _targetState;
	}
}
