/// <summary>
/// A class that stores the progress of the subquest.
/// </summary>
public class SubquestProgress
{
	/// <summary>
	/// Gets the subquest itself.
	/// </summary>
	public Subquest subquest { get; }

	/// <summary>
	/// Gets/sets the state of the subquest.
	/// </summary>
	public SubquestState state { get; set; }

	public SubquestProgress(Subquest subquest, SubquestState state)
	{
		this.subquest = subquest;
		this.state = state;
	}
}

