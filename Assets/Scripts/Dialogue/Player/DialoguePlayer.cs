using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Localization.Tables;

/// <summary>
/// A component that is responsible for playing the dialogue nodes
/// and managing their state.
/// </summary>
public class DialoguePlayer : MonoBehaviour
{
	public const float phraseSkipDelay = 0.2f;

	private DialogueNode _currentNode;
	private readonly List<DialoguePreloadedPhrase> _preloadedPhrases = new();
	private readonly List<string> _speakerNames = new();

	// Texts for each option after converting to the current locale
	private readonly List<string> _optionTexts = new();
	// Available options of the current node
	private readonly List<DialogueOption> _options = new();
	// Nodes to which options are mapped
	private readonly List<DialogueNode> _optionNodes = new();

	private IDialogueListener _listener;
	private bool _isPlaying = false;
	private bool _skipPhrase = false;

	[SerializeField]
	[Tooltip("An array of speakers. The first one is usually the player (main character).")]
	private DialogueSpeakerBase[] _speakers;

	[SerializeField]
	[Tooltip("An event that is triggered when the dialogue has started playing.")]
	private UnityEvent _started;
	[SerializeField]
	[Tooltip("An event that is triggered when the dialogue has ended playing.")]
	private UnityEvent _ended;

	/// <summary>
	/// Gets a flag indicating whether the component is busy playing
	/// a dialogue.
	/// </summary>
	public bool isPlaying => _isPlaying;

	/// <summary>
	/// An event that is triggered when the dialogue has started playing.
	/// </summary>
	public UnityEvent started => _started;
	/// <summary>
	/// An event that is triggered when the dialogue has ended playing.
	/// </summary>
	public UnityEvent ended => _ended;

	private void End()
	{
		_listener.OnEnded(_currentNode);
		_listener = null;
		_isPlaying = false;

		_ended.Invoke();
	}

	private float GetPhraseDuration(DialoguePhrase phrase,
		DialoguePreloadedPhrase loadedPhrase)
	{
		if (loadedPhrase.audioClip && phrase.duration <= 0f)
			return loadedPhrase.audioClip.length;

		return phrase.duration;
	}

	private IEnumerator PreloadPhrases()
	{
		_preloadedPhrases.Clear();
		_speakerNames.Clear();
		_optionTexts.Clear();

		foreach (var speaker in _speakers)
		{
			if (speaker.speakerName.TableReference.ReferenceType != TableReference.Type.Empty)
			{
				var nameAsyncOperation = speaker.speakerName.GetLocalizedStringAsync();
				yield return new WaitUntil(() => nameAsyncOperation.IsDone);
				_speakerNames.Add(nameAsyncOperation.Result);
			}
			else
			{
				_speakerNames.Add(null);
			}
		}

		foreach (var phrase in _currentNode.phrases)
		{
			AudioClip audio = null;
			if (phrase.audioClip != null && phrase.audioClip.TableReference.ReferenceType != TableReference.Type.Empty)
			{
				var audioAsyncOperation = phrase.audioClip.LoadAssetAsync<AudioClip>();
				yield return new WaitUntil(() => audioAsyncOperation.IsDone);
				audio = audioAsyncOperation.Result;
			}

			var textAsyncOperation = phrase.text.GetLocalizedStringAsync();
			yield return new WaitUntil(() => textAsyncOperation.IsDone);
			var text = textAsyncOperation.Result;

			DialoguePreloadedPhrase loadedPhrase = new(audio, text);
			_preloadedPhrases.Add(loadedPhrase);
		}

		foreach (var option in _options)
		{
			var optionAsyncOperation = option.text.GetLocalizedStringAsync();
			yield return new WaitUntil(() => optionAsyncOperation.IsDone);
			_optionTexts.Add(optionAsyncOperation.Result);
		}
	}

	private void PlayPhrase(DialoguePhrase phrase,
		DialoguePreloadedPhrase preloadedPhrase, float duration)
	{
		_speakers[phrase.speakerIndex].PlayPhrase(preloadedPhrase);

		string subtitle = preloadedPhrase.text;
		string speakerName = _speakerNames[phrase.speakerIndex];
		SubtitlesDisplayer.instance.Display(
			subtitle, duration, SubtitlePriority.Dialogue, speakerName);
	}

	private void CancelPhrase(DialoguePhrase phrase)
	{
		_speakers[phrase.speakerIndex].Stop();
		SubtitlesDisplayer.instance.Cancel();
	}

	private IEnumerator PlayPhrases()
	{
		// Wait for the speaker names to load
		yield return new WaitUntil(() => _speakerNames.Count == _speakers.Length);

		for (int i = 0; i < _currentNode.phrases.Count; i++)
		{
			var phrase = _currentNode.phrases[i];
			if (phrase.speakerIndex >= _speakers.Length)
			{
				Debug.LogWarning($"The speakerIndex {phrase.speakerIndex} is invalid. " +
					$"The phrase (index: {i}) is skipped.", gameObject);
				continue;
			}
			DialoguePhraseContext phraseContext = new(_currentNode, phrase, i);

			// Wait for the phrase to load
			yield return new WaitUntil(() => _preloadedPhrases.Count >= i + 1);

			_listener.OnPhraseStarted(phraseContext);

			var preloadedPhrase = _preloadedPhrases[i];
			float duration = GetPhraseDuration(phrase, preloadedPhrase);

			PlayPhrase(phrase, preloadedPhrase, duration);

			// Wait for the phrase to end
			_skipPhrase = false;
			if (!phrase.unskippable) InputSystem.onEvent += OnInputEvent;
			for (float t = 0f; t < duration; t += Time.deltaTime)
			{
				if (t >= phraseSkipDelay && _skipPhrase)
				{
					CancelPhrase(phrase);
					break;
				}
				_skipPhrase = false;

				yield return null;
			}
			if (!phrase.unskippable) InputSystem.onEvent -= OnInputEvent;

			_listener.OnPhraseEnded(phraseContext);
		}

		if (_options.Count > 0)
		{
			yield return new WaitUntil(() => _optionTexts.Count >= _options.Count);

			DialogueOptionController.instance.RequestOption(_optionTexts,
				_currentNode.answerParams, OnOptionSelected);
		}
		else
		{
			DialogueNode next = _currentNode.branching.SelectNode();
			if (!next) next = _currentNode;

			_listener.OnNodeEnded(_currentNode, next);
			End();
		}
	}

	private void OnOptionSelected(int optionIndex)
	{
		var option = _options[optionIndex];
		var nextNode = _optionNodes[optionIndex];

		_listener.OnNodeEnded(_currentNode, nextNode);

		PlayNode(nextNode);

		DialogueOptionContext context = new(_currentNode, option, optionIndex);
		_listener.OnOptionChosen(context);
	}

	private void PlayNode(DialogueNode node)
	{
		_currentNode = node;
		_options.Clear();
		_optionNodes.Clear();

		foreach (var option in node.options)
		{
			var optionNode = option.branching.SelectNode();
			if (optionNode)
			{
				_options.Add(option);
				_optionNodes.Add(optionNode);
			}
		}

		StartCoroutine(PreloadPhrases());
		StartCoroutine(PlayPhrases());
	}

	private void InternalPlay(DialogueNode node, IDialogueListener listener)
	{
		_listener = listener;
		_isPlaying = true;

		PlayNode(node);

		_started.Invoke();
	}

	public void Play(DialogueNode node, IDialogueListener listener = null)
	{
		if (!node)
		{
			throw new System.ArgumentNullException(nameof(node));
		}
		if (_isPlaying)
		{
			throw new System.InvalidOperationException(
				"Attempted to play a new dialogue when the last one hasn't finished yet.");
		}

		InternalPlay(node, listener);
	}

	private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
	{
		if (InputUtils.AnyKeyDown(eventPtr, device))
		{
			_skipPhrase = true;
		}
	}
}
