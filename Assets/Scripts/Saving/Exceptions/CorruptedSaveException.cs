using System;
using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown when the save data is corrupted
/// (e.g., the JSON file has an invalid syntax).
/// </summary>
public class CorruptedSaveException : Exception
{
	public override string Message => $"The save data is corrupted. {base.Message}";

	public CorruptedSaveException() : base()
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