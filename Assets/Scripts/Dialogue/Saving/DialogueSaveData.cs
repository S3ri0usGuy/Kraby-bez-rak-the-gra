/// <summary>
/// Data structure that hold information about the <see cref="DialogueActor" />
/// state.
/// </summary>
[System.Serializable]
public class DialogueSaveData
{
	/// <summary>
	/// A name of the next node that should be played.
	/// </summary>
	public string nextNodeName { get; set; }
}
