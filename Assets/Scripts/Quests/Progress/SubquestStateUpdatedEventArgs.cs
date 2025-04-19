public class SubquestStateUpdatedEventArgs
{
	public Subquest subquest { get; }
	public SubquestState oldSubquestState { get; }
	public SubquestState newSubquestState { get; }

	public Quest quest => subquest.quest;

	public SubquestStateUpdatedEventArgs(Subquest subquest, SubquestState oldSubquestState, SubquestState newSubquestState)
	{
		this.subquest = subquest;
		this.oldSubquestState = oldSubquestState;
		this.newSubquestState = newSubquestState;
	}
}
