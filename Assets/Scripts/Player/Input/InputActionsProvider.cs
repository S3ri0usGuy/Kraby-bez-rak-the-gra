using UnityEngine;
using ArgumentNullException = System.ArgumentException;
using InvalidOperationException = System.InvalidOperationException;

/// <summary>
/// A component that provides an instance of the <see cref="InputActions" /> class.
/// </summary>
public class InputActionsProvider : SingletonMonoBehaviour<InputActionsProvider>
{
	private InputActions _actions;

	/// <summary>
	/// Gets an instance of the input actions that are meant to be
	/// used by the children objects.
	/// </summary>
	public InputActions actions => _actions ?? (_actions = new());

	private void OnEnable()
	{
		actions.Enable();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public static InputActions RetrieveFor(Component obj)
	{
		if (obj == null)
			throw new ArgumentNullException(nameof(obj));

		var provider = obj.GetComponentInParent<InputActionsProvider>();
		if (!provider)
		{
			throw new InvalidOperationException($"The object has no " +
				$"{nameof(InputActionsProvider)} component in parent.");
		}

		return provider.actions;
	}

	public void SetGameActionsActive(bool active)
	{
		if (active)
		{
			_actions.player.Enable();
		}
		else
		{
			_actions.player.Disable();
		}
	}
}
