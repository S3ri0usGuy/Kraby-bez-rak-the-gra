using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// A class that represents the quest stage.
/// </summary>
[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest Stage")]
public class QuestStage : ScriptableObject
{
	[SerializeField]
	[Tooltip("A localized short description of the objective.")]
	private LocalizedString _description;
	[SerializeField]
	[Tooltip("The quest related to this stage.")]
	private Quest _quest;

	/// <summary>
	/// Gets the short description of the objective.
	/// </summary>
	public LocalizedString description => _description;
	/// <summary>
	/// Gets the quest related to this stage.
	/// </summary>
	public Quest quest => _quest;
}
