/// <summary>
/// Represents a state of the subquest.
/// </summary>
public enum SubquestState
{
	/// <summary>
	/// The subquest has not been yet given.
	/// </summary>
	None = 0,
	/// <summary>
	/// The subquest is in progress (active).
	/// </summary>
	Active = 1,
	/// <summary>
	/// The subquest is completed.
	/// </summary>
	Completed,
	/// <summary>
	/// The subquest is failed.
	/// </summary>
	Failed
}