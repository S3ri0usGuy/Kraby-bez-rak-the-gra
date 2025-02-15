using System.IO;
using UnityEngine;

/// <summary>
/// A save persistance that uses JSON files.
/// </summary>
public sealed class JsonDiskSavePersistence : ISavePersistence
{
	private readonly string _folderPath = Application.persistentDataPath;

	/// <summary>
	/// Creates an instance of the JSON disk provider.
	/// </summary>
	public JsonDiskSavePersistence() { }

	/// <summary>
	/// Creates an instance of the JSON disk provider.
	/// </summary>
	/// <param name="folderPath">Path to the folder where data is stored. Application.persistantDataPath is used by default.</param>
	public JsonDiskSavePersistence(string folderPath)
	{
		_folderPath = folderPath;
	}

	private string GetSavePath(string slotName)
	{
		return $"{Path.Combine(_folderPath, slotName)}.json";
	}

	public bool Load(SaveSlot slot)
	{
		string path = GetSavePath(slot.name);
		if (!File.Exists(path)) return false;

		string json = File.ReadAllText(path);
		slot.saveData = JsonHelper.Deserialize(json);

		return true;
	}

	public void Save(SaveSlot slot)
	{
		string path = GetSavePath(slot.name);

		string json = JsonHelper.Serialize(slot.saveData);
		File.WriteAllText(path, json);
	}

	public bool Delete(string slotName)
	{
		string path = GetSavePath(slotName);

		if (File.Exists(path))
		{
			File.Delete(path);
			return true;
		}

		return false;
	}
}
