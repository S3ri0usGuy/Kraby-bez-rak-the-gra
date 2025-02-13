/// <summary>
/// An interface that provides methods for loading and saving game data.
/// </summary>
public interface ISaveProvider
{
	/// <summary>
	/// Loads data into the provided save slot.
	/// </summary>	
	/// <param name="slot">The slot which will be populated with data.</param>
	/// <remarks>
	/// This method does not change the save slot if its data was not found.
	/// </remarks>
	/// <returns>
	/// <see langword="true" /> is the save was successfully loaded;
	/// otherwise <see langword="false" /> if the save was not found.
	/// </returns>
	/// <exception cref="CorruptedSaveException">
	/// Thrown when the data is found but it cannot be loaded.
	/// </exception>
	public bool Load(SaveSlot slot);

	/// <summary>
	/// Saves the provided save slot.
	/// </summary>
	/// <param name="slot">The save slot to be saved.</param>
	public void Save(SaveSlot slot);

	/// <summary>
	/// Deletes the save slot.
	/// </summary>
	/// <param name="slotName">The name of the save slot to be deleted.</param>
	/// <returns>
	/// <see langword="true" /> is the save was successfully deleted;
	/// otherwise <see langword="false" /> if the save to delete was not found.
	/// </returns>
	public bool Delete(string slotName);
}
