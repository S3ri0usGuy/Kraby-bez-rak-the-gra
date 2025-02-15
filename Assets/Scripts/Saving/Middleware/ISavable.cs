using System;

/// <summary>
/// An interface that represents an object that interacts
/// with the save system (usually a Unity component).
/// </summary>
/// <remarks>
/// <para>
/// The most prominent class that implements this interface is the
/// <see cref="SavableComponent{TData}" />.
/// </para>
/// <para>
/// When implementing, make sure that the equals method is correct.
/// For all C# classes without custom Equals override, and Unity objects this should not
/// be a problem.
/// </para>
/// </remarks>
public interface ISavable
{
	/// <summary>
	/// Gets a unique identifier which is assigned to this object.
	/// </summary>
	public string id { get; }

	/// <summary>
	/// Loads a data into the object.
	/// </summary>
	/// <remarks>
	/// Called when the object registers itself in the save system.
	/// </remarks>
	/// <param name="data">
	/// An object to be loaded. Can be <see langword="null" /> if
	/// the data was not found.
	/// </param>
	public void Load(object data);

	/// <summary>
	/// Retrieves a data to be saved.
	/// </summary>
	/// <returns>
	/// An object which is needed to be saved.
	/// </returns>
	public object Save();
}
