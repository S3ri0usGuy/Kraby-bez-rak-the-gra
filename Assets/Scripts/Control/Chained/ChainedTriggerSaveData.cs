using System;

[Serializable]
public class ChainedTriggerSaveData
{
	/// <summary>
	/// Gets/sets a flag that determines whether the chained trigger
	/// has been triggered.
	/// </summary>
	public bool wasTriggered { get; set; }
}
