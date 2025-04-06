using UnityEngine;

/// <summary>
/// A component that helps to lock the player. Locking means that the player
/// loses the control of the player character and the cursor becomes visible.
/// </summary>
public static class PlayerLocker
{
	/// <summary>
	/// Locks the player by disabling input, freezing the movement and enabling the cursor.
	/// </summary>
	/// <remarks>
	/// Calling this method twice without unlocking will lead to an unexpected behaviour.
	/// </remarks>
	public static void Lock()
	{
		if (InputActionsProvider.exists)
		{
			InputActionsProvider.instance.SetGameActionsActive(false);
		}
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		if (Player.exists)
		{
			Player player = Player.instance;
			player.movement.Freeze();
		}
	}

	/// <summary>
	/// Locks the player by enabling input, unfreezing the movement and locking the cursor.
	/// </summary>
	public static void Unlock()
	{
		if (InputActionsProvider.exists)
		{
			InputActionsProvider.instance.SetGameActionsActive(true);
		}
		if (Player.exists)
		{
			Player.instance.movement.Unfreeze();
		}
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}
