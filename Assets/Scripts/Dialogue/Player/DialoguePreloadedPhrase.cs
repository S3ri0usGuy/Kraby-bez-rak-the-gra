using UnityEngine;

public class DialoguePreloadedPhrase
{
	public AudioClip audioClip { get; }
	public string text { get; }

	public DialoguePreloadedPhrase(AudioClip audioClip, string text)
	{
		this.audioClip = audioClip;
		this.text = text;
	}
}
