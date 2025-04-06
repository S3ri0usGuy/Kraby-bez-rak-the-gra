using System;
using UnityEngine;

/// <summary>
/// A base class for the component that provides an event that is triggered by 
/// some action.
/// </summary>
public abstract class Trigger : MonoBehaviour
{
	public event Action<Trigger> triggered;

	/// <summary>
	/// Invokes the <see cref="triggered" /> event.
	/// </summary>
	public void InvokeTriggered()
	{
		triggered?.Invoke(this);
	}
}
