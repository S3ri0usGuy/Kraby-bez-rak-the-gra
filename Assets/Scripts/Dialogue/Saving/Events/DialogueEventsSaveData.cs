using System;

/// <summary>
/// Data structure that stores saved dialogue event keys.
/// </summary>
[Serializable]
public class DialogueEventsSaveData
{
	/// <summary>
	/// Gets/sets all event keys that were saved.
	/// </summary>
	public string[] keys { get; set; }
}
