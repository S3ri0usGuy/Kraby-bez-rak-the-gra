using UnityEditor;

public static class DialogueTools
{
	[MenuItem("Game Design/Dialogues/Turn Off Forced Skippable", true)]
	public static bool ValidateTurnOffForcedSkippable()
	{
		return DialoguePlayer.forcedSkippable;
	}
	[MenuItem("Game Design/Dialogues/Turn Off Forced Skippable", false, 50)]
	public static void TurnOffForcedSkippable()
	{
		DialoguePlayer.forcedSkippable = false;
	}

	[MenuItem("Game Design/Dialogues/Turn On Forced Skippable", true)]
	public static bool ValidateTurnOnForcedSkippable()
	{
		return !DialoguePlayer.forcedSkippable;
	}
	[MenuItem("Game Design/Dialogues/Turn On Forced Skippable", false, 50)]
	public static void TurnOnForcedSkippable()
	{
		DialoguePlayer.forcedSkippable = true;
	}
}
