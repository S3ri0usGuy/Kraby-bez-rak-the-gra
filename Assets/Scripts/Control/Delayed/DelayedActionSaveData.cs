using System;

[Serializable]
public class DelayedActionSaveData
{
	/// <summary>
	/// Gets/sets a time left for the action to be executed.
	/// </summary>
	/// <remarks>
	/// It's 0 or negative if the action has not been started.
	/// </remarks>
	public float timeLeft;
}
