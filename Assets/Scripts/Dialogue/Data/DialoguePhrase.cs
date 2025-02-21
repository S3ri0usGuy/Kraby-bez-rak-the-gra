using System;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Represents a dialogue phrase.
/// </summary>
[Serializable]
public class DialoguePhrase
{
	[SerializeField]
	[Tooltip("An index of the speaker who says this phrase. " +
		"Usually, it's 0 for the player and 1 for the NPC.")]
	private int _speakerIndex;
	[SerializeField]
	[Tooltip("A localized text of the phrase.")]
	private LocalizedString _text;
	[SerializeField]
	[Tooltip("An optional audio clip that will be played.")]
	private LocalizedAudioClip _audioClip;
	[SerializeField]
	[Tooltip("A duration of the phrase in seconds. " +
		"If the audio clip is assigned and this value is zero or negative, " +
		"the clip's duration is taken instead.")]
	private float _duration;

	/// <summary>
	/// Gets an index of the speaker who says this phrase.
	/// Usually, it's 0 for the player and 1 for the NPC.
	/// </summary>
	public int speakerIndex => _speakerIndex;
	/// <summary>
	/// Gets a localized string used for this phrase.
	/// </summary>
	public LocalizedString text => _text;
	/// <summary>
	/// Gets an audio clip that is played for this phrase.
	/// </summary>
	public LocalizedAudioClip audioClip => _audioClip;
	/// <summary>
	/// Gets a duration of the phrase.
	/// </summary>
	/// <remarks>
	/// This property is not used if it's zero or negative and 
	/// the <see cref="audioClip" /> is assigned.
	/// </remarks>
	public float duration => _duration;
}
