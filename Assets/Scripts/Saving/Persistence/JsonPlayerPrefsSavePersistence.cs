using UnityEngine;

/// <summary>
/// A save persistance that uses <see cref="PlayerPrefs"> and JSON format.
/// </summary>
public sealed class JsonPlayerPrefsSavePersistence : ISavePersistence
{
	/// <summary>
	/// Creates an instance of the JSON disk provider.
	/// </summary>
	public JsonPlayerPrefsSavePersistence() { }

	private string SlotNameToKey(string slotName)
	{
		return $"save:{slotName}";
	}

	public bool Load(SaveSlot slot)
	{
		string key = SlotNameToKey(slot.name);
		if (PlayerPrefs.HasKey(key)) return false;

		string json = PlayerPrefs.GetString(key);
		slot.saveData = JsonHelper.Deserialize(json);

		return true;
	}

	public void Save(SaveSlot slot)
	{
		string key = SlotNameToKey(slot.name);

		string json = JsonHelper.Serialize(slot.saveData);
		PlayerPrefs.SetString(key, json);
	}

	public bool Delete(string slotName)
	{
		string key = SlotNameToKey(slotName);

		if (PlayerPrefs.HasKey(key))
		{
			PlayerPrefs.DeleteKey(key);
			return true;
		}

		return false;
	}
}
