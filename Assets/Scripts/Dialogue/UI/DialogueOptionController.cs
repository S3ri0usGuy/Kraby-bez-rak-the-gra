using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component that is responsible for the dialogue options input
/// and UI visualization.
/// </summary>
public class DialogueOptionController : SingletonMonoBehaviour<DialogueOptionController>
{
	private DialogueOptionCallback _callback;
	private DialogueOptionButton[] _optionButtons;

	private int _availableOptionsCount;

	[SerializeField]
	private GameObject _window;
	[SerializeField]
	[Tooltip("A prefab for the dialogue option button.")]
	private DialogueOptionButton _optionButtonPrefab;
	[SerializeField]
	private Transform _dialogueOptionsParent;
	[SerializeField]
	private ProgressTimer _timer;

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
		SelectOption(button.optionIndex);
	}

	private void SetButtonsActive(int count)
	{
		for (int i = 0; i < _optionButtons.Length; i++)
		{
			_optionButtons[i].gameObject.SetActive(i < count);
		}
	}

	private IEnumerator ForcedChoiceCountdown(DialogueAnswerParams answerParams, int optionsCount)
	{
		if (answerParams.answerType == DialogueAnswerType.Default)
		{
			if (_timer) _timer.gameObject.SetActive(false);
			yield break;
		}

		if (_timer)
		{
			_timer.gameObject.SetActive(true);
			_timer.StartCountdown(answerParams.timeToAnswer);
		}

		yield return new WaitForSeconds(answerParams.timeToAnswer);

		int choiceIndex = answerParams.answerType switch
		{
			DialogueAnswerType.TimedPickFirst => 0,
			DialogueAnswerType.TimedPickRandom => Random.Range(0, optionsCount),

			_ => throw new System.ArgumentException(
				$"Invalid answer type ({(int)answerParams.answerType}).",
				nameof(answerParams)
				)
		};
		SelectOption(choiceIndex);

		if (_timer)
		{
			_timer.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Enables the player to select a dialogue option.
	/// </summary>
	/// <param name="options">A collection of all available options.</param>
	/// <param name="answerParams">Behaviour parameters.</param>
	/// <param name="callback">
	/// A one-time callback that is called when the option is selected.
	/// </param>
	/// <exception cref="System.ArgumentNullException" />
	public void RequestOption(IReadOnlyList<string> options,
		DialogueAnswerParams answerParams, DialogueOptionCallback callback)
	{
		if (answerParams == null) throw new System.ArgumentNullException(nameof(answerParams));

		_callback = callback ?? throw new System.ArgumentNullException(nameof(callback));

		StopAllCoroutines();

		SetButtonsActive(options.Count);
		_availableOptionsCount = options.Count;
		for (int i = 0; i < options.Count; i++)
		{
			_optionButtons[i].SetText(options[i]);
		}

		_window.SetActive(true);

		StartCoroutine(ForcedChoiceCountdown(answerParams, options.Count));
	}

	/// <summary>
	/// Selects the option by its index.
	/// </summary>
	/// <remarks>
	/// Does nothing if the index is invalid or there is no options to select.
	/// </remarks>
	/// <param name="optionIndex">
	/// An index of the option to choose from 0 to
	/// <see cref="DialogueNode.maxDialogueOptions" /> - 1.
	/// </param>
	/// <exception cref="System.ArgumentOutOfRangeException">
	/// The <paramref name="optionIndex"/> is out of the range.
	/// </exception>
	public void SelectOption(int optionIndex)
	{
		if (optionIndex < 0 || optionIndex >= DialogueNode.maxDialogueOptions)
		{
			throw new System.ArgumentOutOfRangeException(nameof(optionIndex));
		}

		if (_callback == null || optionIndex >= _availableOptionsCount) return;

		_window.SetActive(false);
		_callback?.Invoke(optionIndex);
		_callback = null; // Make sure it's one-time call only
	}
}
