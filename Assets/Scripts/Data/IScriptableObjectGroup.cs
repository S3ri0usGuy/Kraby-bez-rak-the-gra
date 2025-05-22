/// <summary>
/// An interface that is used for the <see cref="ScriptableObjectGroup{T}" /> to expose
/// the methods without exposing the type parameter.
/// </summary>
public interface IScriptableObjectGroup
{
#if UNITY_EDITOR
	/// <summary>
	/// Loads children objects from the base folder.
	/// </summary>
	public void LoadObjects();
#endif
}
