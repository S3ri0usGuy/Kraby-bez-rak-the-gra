using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A component that stores dialogue save event keys.
/// </summary>
public class DialogueSaveEventsStorage : SavableComponent<DialogueEventsSaveData>
{
	protected override DialogueEventsSaveData fallbackData => new()
	{
		keys = Array.Empty<string>()
	};

	/// <summary>
	/// Gets a hash set containing dialogue save event keys.
	/// </summary>
	public HashSet<string> keys { get; private set; } = new();

	protected override void OnLoad()
	{
		keys = new(data.keys ?? Array.Empty<string>());
	}

	protected override void OnSave()
	{
		data.keys = keys.ToArray();
	}
}
