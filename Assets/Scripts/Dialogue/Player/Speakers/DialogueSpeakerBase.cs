using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// A class that provides a base for the dialogue object that "speaks".
/// </summary>
public abstract class DialogueSpeakerBase : MonoBehaviour
{
	/// <summary>
	/// Gets a localized name of the speaker.
	/// </summary>
	public abstract LocalizedString speakerName { get; }

	/// <summary>
	/// Plays a phrase.
	/// </summary>
	/// <remarks>
	/// The audio clip will be ignored if it's <see langword="null" />.
	/// </remarks>
	/// <param name="phrase">A phrase to be played.</param>
	public abstract void PlayPhrase(DialoguePreloadedPhrase phrase);

	/// <summary>
	/// Cancels playing the current phrase.
	/// </summary>
	/// <remarks>
	/// Does nothing if no phrases are playing.
	/// </remarks>
	public abstract void Stop();
}
