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

	private IDialogueListener _listener;
	private bool _isPlaying = false;

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

	private IEnumerator PreloadPhrases()
	{
		foreach (var phrase in _currentNode.phrases)
		{
			var audioAsyncOperation = phrase.audioClip.LoadAssetAsync<AudioClip>();
			yield return new WaitUntil(() => audioAsyncOperation.IsDone);
			var audio = audioAsyncOperation.Result;

			var textAsyncOperation = phrase.text.GetLocalizedStringAsync();
			yield return new WaitUntil(() => textAsyncOperation.IsDone);
			var text = textAsyncOperation.Result;

			DialoguePreloadedPhrase loadedPhrase = new(audio, text);
			_preloadedPhrases.Add(loadedPhrase);
		}
	}

	private IEnumerator PlayPhrases()
	{
		for (int i = 0; i < _currentNode.phrases.Count; i++)
		{
			yield return new WaitUntil(() => _preloadedPhrases.Count >= i + 1);

			Debug.Log($"Speaker #{_currentNode.phrases[i].personId}: {_preloadedPhrases[i].text}");

			// TODO: subtitles

			// TODO: play audio

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

		_preloadedPhrases.Clear();
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
