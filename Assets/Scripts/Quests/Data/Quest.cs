using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// An object that represents a quest.
/// </summary>
[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
	[SerializeField]
	[Tooltip("A localized name of the quest.")]
	private LocalizedString _name;

	/// <summary>
	/// Gets a localized name of the quest.
	/// </summary>
	public LocalizedString questName => _name;
}
