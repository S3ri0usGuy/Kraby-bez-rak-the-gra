using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// A component that represents an object that "speaks".
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class DialogueSpeaker : MonoBehaviour
{
	private AudioSource _audioSource;

	[SerializeField]
	[Tooltip("A localized name of the speaker.")]
	private LocalizedString _name;

	/// <summary>
	/// Gets a localized name of the speaker.
	/// </summary>
	public new LocalizedString name => _name;

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
	public void PlayPhrase(DialoguePreloadedPhrase phrase)
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
	public void Stop()
	{
		_audioSource.Stop();
	}
}
