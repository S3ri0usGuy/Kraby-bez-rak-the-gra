using UnityEngine.Localization;

/// <summary>
/// A dialogue speaker that always points to the <see cref="Player.speaker" />.
/// </summary>
public class DialogueSpeakerPlayer : DialogueSpeakerBase
{
	public override LocalizedString speakerName => Player.instance.speaker.speakerName;

	public override void PlayPhrase(DialoguePreloadedPhrase phrase)
	{
		Player.instance.speaker.PlayPhrase(phrase);
	}

	public override void Stop()
	{
		Player.instance.speaker.Stop();
	}
}
