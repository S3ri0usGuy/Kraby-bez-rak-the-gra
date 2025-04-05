/// <summary>
/// Represents a state of the quest stage.
/// </summary>
public enum QuestState
{
	/// <summary>
	/// The quest has not been yet given.
	/// </summary>
	None = 0,
	/// <summary>
	/// The stage is in progress (active).
	/// </summary>
	Active = 1,
	/// <summary>
	/// The stage is completed.
	/// </summary>
	Completed,
	/// <summary>
	/// The stage is failed.
	/// </summary>
	Failed
}
