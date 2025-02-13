/// <summary>
/// A factory class which creates a save provider for the current platform.
/// </summary>
public static class SavePersistenceFactory
{
	/// <summary>
	/// Creates a save persistence suitable for the current platform.
	/// </summary>
	/// <returns>
	/// A save persistence suitable for the current platform.
	/// </returns>
	public static ISavePersistence CreatePersistence()
	{
#if UNITY_WEBGL
		return new JsonPlayerPrefsSavePersistence();
#else
		return new JsonDiskSavePersistence();
#endif
	}
}
