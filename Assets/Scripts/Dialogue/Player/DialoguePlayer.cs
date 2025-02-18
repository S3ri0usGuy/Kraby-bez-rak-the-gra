using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that is responsible for playing the dialogue nodes
/// and managing their state.
/// </summary>
public class DialoguePlayer : MonoBehaviour
{
	private DialogueNode _currentNode;
	private readonly List<DialoguePreloadedPhrase> _preloadedPhrases = new();
	private readonly List<string> _speakerNames = new();

	private IDialogueListener _listener;
	private bool _isPlaying = false;

	[SerializeField]
	[Tooltip("An array of speakers. The first one is usually the player (main character).")]
	private DialogueSpeaker[] _speakers;

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

		foreach (var speaker in _speakers)
		{
			var nameAsyncOperation = speaker.name.GetLocalizedStringAsync();
			yield return new WaitUntil(() => nameAsyncOperation.IsDone);
			_speakerNames.Add(nameAsyncOperation.Result);
		}

		foreach (var phrase in _currentNode.phrases)
		{
			AudioClip audio = null;
			if (phrase.audioClip != null)
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

			// Wait for the phrase to load
			yield return new WaitUntil(() => _preloadedPhrases.Count >= i + 1);

			var preloadedPhrase = _preloadedPhrases[i];
			_speakers[phrase.speakerIndex].PlayPhrase(preloadedPhrase);

			Debug.Log($"Speaker #{phrase.speakerIndex}: {_preloadedPhrases[i].text}");

			float duration = GetPhraseDuration(phrase, preloadedPhrase);
			yield return new WaitForSeconds(duration);

			// TODO: add callbacks

			// TODO: add options
		}
	}

	private void InternalPlay(DialogueNode node, IDialogueListener listener)
	{
		// TODO: switch camera position
		// TODO: block input

		_listener = listener;
		_isPlaying = true;
		_currentNode = node;

		StartCoroutine(PreloadPhrases());

		StartCoroutine(PlayPhrases());

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
}
