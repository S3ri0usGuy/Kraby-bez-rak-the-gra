using UnityEngine;
using UnityEngine.InputSystem;

public class ExitOnEscape : MonoBehaviour
{
	private InputActionsProvider _provider;

	private void Awake()
	{
		_provider = this.TryGetInParent<InputActionsProvider>();
	}

	private void OnEnable()
	{
		_provider.actions.system.pause.performed += OnPause;
	}

	private void OnDisable()
	{
		_provider.actions.system.pause.performed -= OnPause;
	}

	private void OnPause(InputAction.CallbackContext context)
	{
		Application.Quit();
		Debug.Log("The player fucking rage quitted the game!");
	}
}
