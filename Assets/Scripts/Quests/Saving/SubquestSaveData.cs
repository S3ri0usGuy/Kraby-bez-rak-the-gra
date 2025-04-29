using System;

[Serializable]
public class SubquestSaveData
{
	/// <summary>
	/// Gets/sets a name of the subquest.
	/// </summary>
	public string name { get; set; }

	/// <summary>
	/// Gets/sets the subquest state.
	/// </summary>
	public SubquestState state { get; set; }
}
