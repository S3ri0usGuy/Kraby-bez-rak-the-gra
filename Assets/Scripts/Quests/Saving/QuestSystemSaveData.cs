using System.Collections.Generic;

/// <summary>
/// Data structure that holds information about the <see cref="QuestSystem" />
/// state.
/// </summary>
[System.Serializable]
public class QuestSystemSaveData
{
	/// <summary>
	/// Gets/sets the list of the quests save data.
	/// </summary>
	public List<QuestSaveData> quests { get; set; }
	/// <summary>
	/// Gets/sets the list of the subquests save data.
	/// </summary>
	public List<SubquestSaveData> subquests { get; set; }
}
