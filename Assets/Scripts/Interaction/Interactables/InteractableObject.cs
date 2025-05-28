using System;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// A base class for interactable object components.
/// </summary>
public abstract class InteractableObject : MonoBehaviour
{
	[SerializeField]
	[Tooltip("An action name displayed on the UI canvas.")]
	private LocalizedString _actionName;

	[SerializeField]
	[Tooltip("If checked, then this object will not be interactable when the time was spent.")]
	private bool _requiresTime = true;

	public event Action<InteractableObject> interacted;

	/// <summary>
	/// Gets an action name displayed on the UI canvas.
	/// </summary>
	public virtual LocalizedString actionName { get => _actionName; set => _actionName = value; }

	/// <summary>
	/// Checks whether the object is interactable at this moment.
	/// </summary>
	/// <param name="player">A player that wants to interact with the object.</param>
	/// <returns>
	/// A flag determining whether the object is interactable at this moment.
	/// </returns>
	protected abstract bool CanBeInteractedWith(Player player);

	/// <summary>
	/// Checks whether the object is interactable at this moment.
	/// </summary>
	/// <param name="player">A player that wants to interact with the object.</param>
	/// <returns>
	/// A flag determining whether the object is interactable at this moment.
	/// </returns>
	public bool IsInteractable(Player player)
	{
		if (Clock.exists && _requiresTime && Clock.instance.minutesLeft <= 0)
		{
			return false;
		}

		return CanBeInteractedWith(player);
	}

	protected abstract bool PerformInteract(Player player);

	/// <summary>
	/// A method that is called when the player presses the "interact" button.
	/// When overriden, performs some action.
	/// </summary>
	/// <param name="player">A player that interacts with the object.</param>
	/// <returns>
	/// <see langword="true" /> if this object must be "forgotten about" after an
	/// interaction; otherwise <see langword="false" />.
	/// </returns>
	public bool Interact(Player player)
	{
		bool result = PerformInteract(player);
		interacted?.Invoke(this);
		return result;
	}
}
