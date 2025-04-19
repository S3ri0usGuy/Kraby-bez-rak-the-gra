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
	[SerializeField]
	[Tooltip("If checked, the quest and active subquests will fail when the time is over.")]
	private bool _failOnTimeOver = true;

	/// <summary>
	/// Gets a localized name of the quest.
	/// </summary>
	public LocalizedString questName => _name;

	/// <summary>
	/// Gets a boolean indicating whether the quest and its active subquest must fail
	/// after the time is over.
	/// </summary>
	public bool failOnTimeOver => _failOnTimeOver;
}
