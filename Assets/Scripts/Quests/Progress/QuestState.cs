/// <summary>
/// Represents a state of the quest.
/// </summary>
public enum QuestState
{
	/// <summary>
	/// The quest has not been yet given.
	/// </summary>
	None = 0,
	/// <summary>
	/// The quest is in progress (active).
	/// </summary>
	Active = 1,
	/// <summary>
	/// The quest is completed.
	/// </summary>
	Completed,
	/// <summary>
	/// The quest is failed.
	/// </summary>
	Failed
}
