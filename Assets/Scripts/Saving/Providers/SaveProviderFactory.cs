/// <summary>
/// A factory class which creates a save provider for the current platform.
/// </summary>
public static class SaveProviderFactory
{
	/// <summary>
	/// Creates a save provider suitable for the current platform.
	/// </summary>
	/// <returns>
	/// A save provider suitable for the current platform.
	/// </returns>
	public static ISaveProvider CreateProvider()
	{
		return new JsonDiskSaveProvider();
	}
}
