using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An interactable object that calls an event when interacted with.
/// </summary>
public class EventInteractable : InteractableObject
{
	[SerializeField]
	private UnityEvent _event;

	protected override bool CanBeInteractedWith(Player player)
	{
		return true;
	}

	public override bool Interact(Player player)
	{
		_event.Invoke();
		return true;
	}
}
