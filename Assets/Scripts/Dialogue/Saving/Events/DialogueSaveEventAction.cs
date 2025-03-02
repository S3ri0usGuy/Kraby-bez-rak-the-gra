using UnityEngine;

/// <summary>
/// A component that represents a save event action. It's intended to 
/// be used together with the <see cref="DialogueNodeTrigger"/> in 
/// the Unity events.
/// </summary>
public class DialogueSaveEventAction : MonoBehaviour
{
	public enum EventAction
	{
		Add,
		Remove
	}

	[SerializeField]
	[Tooltip("An event on which the action is performed.")]
	private DialogueSaveEvent _event;

	[SerializeField]
	[Tooltip("An action that is performed on the save event.")]
	private EventAction _action = EventAction.Add;

	/// <summary>
	/// Performs the action.
	/// </summary>
	public void Perform()
	{
		if (!_event)
		{
			Debug.LogError($"The dialogue save event action (\"{gameObject.name}\") " +
				$"has no dialogue event assigned to it.", gameObject);
			return;
		}

		switch (_action)
		{
			case EventAction.Add:
				DialogueSaveEventsProvider.instance.AddKey(_event.id);
				break;
			case EventAction.Remove:
				DialogueSaveEventsProvider.instance.RemoveKey(_event.id);
				break;

			default:
				Debug.Log($"Invalid event action: {(int)_action}.", gameObject);
				break;
		}
	}
}
