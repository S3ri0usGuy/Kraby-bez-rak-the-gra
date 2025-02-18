using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component that is responsible for the dialogue options input
/// and UI visualization.
/// </summary>
public class DialogueOptionController : SingletonMonoBehaviour<DialogueOptionController>
{
	// TODO: add input handling to allow players
	// press a button to pick an option instead of clicking

	private DialogueOptionCallback _callback;
	private DialogueOptionButton[] _optionButtons;

	[SerializeField]
	private GameObject _window;
	[SerializeField]
	[Tooltip("A prefab for the dialogue option button.")]
	private DialogueOptionButton _optionButtonPrefab;
	[SerializeField]
	private Transform _dialogueOptionsParent;

	private void Start()
	{
		_optionButtons = new DialogueOptionButton[DialogueNode.maxDialogueOptions];
		for (int i = 0; i < _optionButtons.Length; i++)
		{
			var button = Instantiate(_optionButtonPrefab, _dialogueOptionsParent);
			_optionButtons[i] = button;

			button.optionIndex = i;
			button.clicked += OnButtonClicked;
		}
	}

	private void OnButtonClicked(DialogueOptionButton button)
	{
		_window.SetActive(false);
		_callback?.Invoke(button.optionIndex);
		_callback = null; // Make sure it's one-time call only
	}

	private void SetButtonsActive(int count)
	{
		for (int i = 0; i < _optionButtons.Length; i++)
		{
			_optionButtons[i].gameObject.SetActive(i < count);
		}
	}

	/// <summary>
	/// Enables the player to select a dialogue option.
	/// </summary>
	/// <param name="callback">
	/// A one-time callback that is called when the option is selected.
	/// </param>
	public void RequestOption(IReadOnlyList<string> options, DialogueOptionCallback callback)
	{
		_callback = callback ?? throw new System.ArgumentNullException(nameof(callback));

		SetButtonsActive(options.Count);
		for (int i = 0; i < options.Count; i++)
		{
			_optionButtons[i].SetText(options[i]);
		}

		_window.SetActive(true);
	}
}
