using System;
using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown when the save data is corrupted
/// (e.g., the JSON file has an invalid syntax).
/// </summary>
public class CorruptedSaveException : Exception
{
	public CorruptedSaveException()
		: base("The save data is corrupted.")
	{ }

	public CorruptedSaveException(string message)
		: base(message)
	{ }

	public CorruptedSaveException(string message, Exception innerException)
		: base(message, innerException)
	{ }

	protected CorruptedSaveException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{ }
}