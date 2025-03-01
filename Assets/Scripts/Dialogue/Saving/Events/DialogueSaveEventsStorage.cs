using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A component that stores dialogue save event keys.
/// </summary>
public class DialogueSaveEventsStorage : SavableComponent<DialogueEventsSaveData>
{
	/// <summary>
	/// Gets a hash set containing dialogue save event keys.
	/// </summary>
	public HashSet<string> keys { get; private set; }

	protected override void OnLoad()
	{
		keys = new(data.keys);
	}

	protected override void OnSave()
	{
		data.keys = keys.ToArray();
	}
}
