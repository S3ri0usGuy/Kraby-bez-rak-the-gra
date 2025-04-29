using UnityEngine;

/// <summary>
/// A component that provides a default shared dialogue group.
/// </summary>
public class SharedDialogueGroupProvider : SingletonMonoBehaviour<SharedDialogueGroupProvider>
{
	[SerializeField]
	private DialogueGroup _sharedGroup;

	/// <summary>
	/// Gets the shared group.
	/// </summary>
	public DialogueGroup sharedGroup => _sharedGroup;
}
