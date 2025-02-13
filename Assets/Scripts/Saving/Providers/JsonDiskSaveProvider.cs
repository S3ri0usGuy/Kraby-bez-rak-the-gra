using System.IO;
using UnityEngine;

/// <summary>
/// A save provider which works with json files on the disk.
/// </summary>
public sealed class JsonDiskSaveProvider : ISaveProvider
{
	private readonly string _folderPath = Application.persistentDataPath;

	/// <summary>
	/// Creates an instance of the JSON disk provider.
	/// </summary>
	public JsonDiskSaveProvider() { }

	/// <summary>
	/// Creates an instance of the JSON disk provider.
	/// </summary>
	/// <param name="folderPath">Path to the folder where data is stored. Application.persistantDataPath is used by default.</param>
	public JsonDiskSaveProvider(string folderPath)
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
