using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A component that allows to choose dialogue options based on the
/// user input.
/// </summary>
public class DialogueOptionInput : MonoBehaviour
{
	private DialogueOptionController _optionController;

	[SerializeField]
	[Tooltip("Options mapped to each index.")]
	private InputActionReference[] _optionActions;

	private void Awake()
	{
		_optionController = this.TryGetInParent<DialogueOptionController>();

		if (_optionActions.Length != DialogueNode.maxDialogueOptions)
		{
			Debug.LogError($"Invalid number of option actions. " +
				$"Expected {DialogueNode.maxDialogueOptions} but was {_optionActions.Length}.");
			return;
		}

		for (int i = 0; i < _optionActions.Length; i++)
		{
			int index = i; // Famous C# delegates bug
			_optionActions[i].action.performed += _ => OnActionPerformed(index);
		}
	}

	private void OnActionPerformed(int index)
	{
		_optionController.SelectOption(index);
	}
}
