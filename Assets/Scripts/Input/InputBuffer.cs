using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class that implements input buffering, allowing for actions to be 
/// performed within a specific time window after input.
/// </summary>
public class InputBuffer
{
	private Coroutine _coroutine;

	private readonly Func<InputAction.CallbackContext, bool> _perform;
	private readonly MonoBehaviour _owner;
	private readonly float _window;

	/// <summary>
	/// The default duration for input buffering window.
	/// </summary>
	public const float defaultWindow = 0.15f;
	/// <summary>
	/// Initializes a new instance of the <see cref="InputBuffer"/> class.
	/// </summary>
	/// <param name="perform">A function that determines if the buffered action can be performed.</param>
	/// <param name="owner">The MonoBehaviour that owns the buffer, required for starting coroutines.</param>
	/// <param name="window">The duration of the input buffer in seconds.</param>
	public InputBuffer(Func<InputAction.CallbackContext, bool> perform, MonoBehaviour owner,
		float window = defaultWindow)
	{
		_perform = perform;
		_owner = owner;
		_window = window;
	}

	private IEnumerator BufferCoroutine(InputAction.CallbackContext context)
	{
		float time = _window;
		while (time > 0f)
		{
			yield return null;

			if (_perform(context)) break;

			time -= Time.deltaTime;
		}
	}

	/// <summary>
	/// Attempts to buffer the action. If the action can't be performed immediately,
	/// it starts the input buffer coroutine to allow delayed execution.
	/// </summary>
	/// <param name="context">The input action callback context.</param>
	public void BufferAction(InputAction.CallbackContext context)
	{
		if (_perform(context)) return;

		if (_coroutine != null) _owner.StopCoroutine(_coroutine);
		_coroutine = _owner.StartCoroutine(BufferCoroutine(context));
	}

	/// <summary>
	/// Input action listener for when an action is performed. This is typically
	/// called by Unity's Input System.
	/// </summary>
	/// <param name="context">The input action callback context.</param>
	public void PerformedListener(InputAction.CallbackContext context)
	{
		BufferAction(context);
	}
}
