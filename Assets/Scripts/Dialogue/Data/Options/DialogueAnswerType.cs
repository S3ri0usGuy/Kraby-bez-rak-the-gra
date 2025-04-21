/// <summary>
/// An enumeration of all available dialogue node answer
/// behaviours.
/// </summary>
public enum DialogueAnswerType
{
	/// <summary>
	/// The player has an infinite time to answer.
	/// </summary>
	Default = 0,
	/// <summary>
	/// The player's time to answer is limited. If time is out,
	/// the first option is picked.
	/// </summary>
	TimedPickFirst = 1,
	/// <summary>
	/// The player's time to answer is limited. If time is out,
	/// a random option is picked.
	/// </summary>
	TimedPickRandom = 2
}
