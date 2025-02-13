using Newtonsoft.Json;
using System;

public static class JsonHelper
{
	private static readonly JsonSerializerSettings _settings = new()
	{
		TypeNameHandling = TypeNameHandling.Auto,
		NullValueHandling = NullValueHandling.Ignore
	};

	/// <summary>
	/// Deserializes the JSON string.
	/// </summary>
	/// <param name="json">The JSON string.</param>
	/// <returns>A deserialized save object.</returns>
	/// <exception cref="CorruptedSaveException" />
	/// <exception cref="ArgumentNullException" />
	public static SaveData Deserialize(string json)
	{
		if (json == null) throw new ArgumentNullException(nameof(json));

		try
		{
			return JsonConvert.DeserializeObject<SaveData>(json, _settings);
		}
		catch (JsonReaderException exception)
		{
			throw new CorruptedSaveException(exception.Message, exception);
		}
		catch (JsonSerializationException exception)
		{
			throw new CorruptedSaveException(exception.Message, exception);
		}
	}

	/// <summary>
	/// Serializes the JSON into a string.
	/// </summary>
	/// <param name="saveData">The save data to be serialized.</param>
	/// <returns>A JSON string with the serialized object.</returns>
	public static string Serialize(SaveData saveData)
	{
		return JsonConvert.SerializeObject(saveData, _settings);
	}
}
