using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// A class that represents the quest stage.
/// </summary>
[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest Stage")]
public class QuestStage : ScriptableObject
{
	public enum StateAction
	{
		/// <summary>
		/// The stage will not change its state (not recommended).
		/// </summary>
		None,
		/// <summary>
		/// The stage will fail.
		/// </summary>
		Fail,
		/// <summary>
		/// The stage will be completed.
		/// </summary>
		Pass
	}

	[SerializeField]
	[Tooltip("A localized short description of the objective.")]
	private LocalizedString _description;
	[SerializeField]
	[Tooltip("The quest related to this stage.")]
	private Quest _quest;

	[SerializeField]
	private StateAction _actionOnQuestPassed = StateAction.Pass;
	[SerializeField]
	private StateAction _actionOnQuestFailed = StateAction.Fail;

	/// <summary>
	/// Gets the short description of the objective.
	/// </summary>
	public LocalizedString description => _description;
	/// <summary>
	/// Gets the quest related to this stage.
	/// </summary>
	public Quest quest => _quest;

	/// <summary>
	/// Gets an action that will automatically happen after the quest is passed.
	/// </summary>
	public StateAction actionOnQuestPassed => _actionOnQuestPassed;

	/// <summary>
	/// Gets an action that will automatically happen after the quest is failed.
	/// </summary>
	public StateAction actionOnQuestFailed => _actionOnQuestFailed;
}
