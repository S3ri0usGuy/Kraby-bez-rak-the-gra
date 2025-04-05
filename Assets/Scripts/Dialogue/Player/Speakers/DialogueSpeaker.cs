using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// A component that represents an object that "speaks".
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class DialogueSpeaker : DialogueSpeakerBase
{
	private AudioSource _audioSource;

	[SerializeField]
	[Tooltip("An optional localized name of the speaker.")]
	private LocalizedString _name;

	public override LocalizedString speakerName => _name;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	/// <summary>
	/// Plays a phrase.
	/// </summary>
	/// <remarks>
	/// The audio clip will be ignored if it's <see langword="null" />.
	/// </remarks>
	/// <param name="phrase">A phrase to be played.</param>
	public override void PlayPhrase(DialoguePreloadedPhrase phrase)
	{
		_audioSource.Stop();
		if (phrase.audioClip)
		{
			_audioSource.clip = phrase.audioClip;
			_audioSource.Play();
		}
		_audioSource.clip = phrase.audioClip;
	}

	/// <summary>
	/// Cancels playing a phrase.
	/// </summary>
	/// <remarks>
	/// Does nothing if no phrases are playing.
	/// </remarks>
	public override void Stop()
	{
		_audioSource.Stop();
	}
}
