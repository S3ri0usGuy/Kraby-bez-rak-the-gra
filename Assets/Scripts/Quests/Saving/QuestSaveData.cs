using System;

[Serializable]
public class QuestSaveData
{
	/// <summary>
	/// Gets/sets a name of the quest.
	/// </summary>
	public string name { get; set; }

	/// <summary>
	/// Gets/sets the quest state.
	/// </summary>
	public QuestState state { get; set; }
}
