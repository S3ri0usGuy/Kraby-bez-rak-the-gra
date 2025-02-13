using System;
using System.Collections.Generic;

/// <summary>
/// A class that represents a game save data that is serialized and deserialized.
/// </summary>
public sealed class SaveData
{
	/// <summary>
	/// Gets/sets a dictionary that maps a unique identifier to the component's data.
	/// </summary>
	public Dictionary<Guid, object> componenentsData { get; set; }
}
