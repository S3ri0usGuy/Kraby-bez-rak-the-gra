using System;
using UnityEngine;
/// <summary>
/// A component that provides player movement without jump mechanics.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class PlayerJump : MonoBehaviour
{
	private PlayerMovement _movement;
	/// <summary>
	/// An event that is triggered when the jump is attempted.
	/// </summary>
	public event Action<PlayerJump> performed;
	private void Awake()
	{
		_movement = GetComponent<PlayerMovement>();
	}
	/// <summary>
	/// This method is no longer needed as jumping is disabled.
	/// </summary>
	public bool Perform()
	{
		// Skok nie jest ju¿ mo¿liwy, wiêc zawsze zwracamy false.
		return false;
	}
}
