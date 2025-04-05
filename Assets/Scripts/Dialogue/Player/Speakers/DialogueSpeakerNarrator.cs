using UnityEngine.Localization;

/// <summary>
/// A dialogue speaker that always points to the <see cref="NarrationScreen.narratorSpeaker" />.
/// </summary>
public class DialogueSpeakerNarrator : DialogueSpeakerBase
{
	public override LocalizedString speakerName => NarrationScreen.instance.narratorSpeaker.speakerName;

	public override void PlayPhrase(DialoguePreloadedPhrase phrase)
	{
		NarrationScreen.instance.narratorSpeaker.PlayPhrase(phrase);
	}

	public override void Stop()
	{
		NarrationScreen.instance.narratorSpeaker.Stop();
	}
}
