using System;
using System.Text.RegularExpressions;

/// <summary>
/// A class that represents a save slot.
/// </summary>
public sealed class SaveSlot
{
	/// <summary>
	/// Gets a name of the save slot.
	/// </summary>
	/// <remarks>
	/// To prevent invalid paths issues, this name should contain only 
	/// alphanumeric characters (including '-' and '_').
	/// </remarks>
	public string name { get; }

	/// <summary>
	/// Gets/sets a save data.
	/// </summary>
	public SaveData saveData { get; set; }

	/// <summary>
	/// Creates an empty slot save.
	/// </summary>
	/// <param name="slotName">Name of the save slot. Alphanumeric characters (including '-' and '_') only. Cannot be empty.</param>
	public SaveSlot(string slotName)
	{
		if (string.IsNullOrEmpty(slotName))
			throw new ArgumentException("Slot name cannot be empty.", nameof(slotName));

		Regex regex = new(@"^[a-zA-Z0-9_-]*$");
		if (!regex.IsMatch(slotName))
			throw new ArgumentException("Slot name should contain alphanumeric characters only.", nameof(slotName));

		name = slotName;
	}
}
