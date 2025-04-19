using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// A class that represents the subquest (a task inside the quest).
/// </summary>
[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Subquest")]
public class Subquest : ScriptableObject
{
	public enum StateAction
	{
		/// <summary>
		/// The subquest will not change its state (not recommended).
		/// </summary>
		None,
		/// <summary>
		/// The subquest will fail.
		/// </summary>
		Fail,
		/// <summary>
		/// The subquest will be completed.
		/// </summary>
		Pass
	}

	[SerializeField]
	[Tooltip("A localized short description of the objective.")]
	private LocalizedString _description;
	[SerializeField]
	[Tooltip("The quest that this subquest is a part of.")]
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
	/// Gets the quest related to this subquest.
	/// </summary>
	public Quest quest => _quest;

	/// <summary>
	/// Gets an action that will automatically happen to this subquest 
	/// after the quest is passed.
	/// </summary>
	public StateAction actionOnQuestPassed => _actionOnQuestPassed;

	/// <summary>
	/// Gets an action that will automatically happen to this subquest
	/// after the quest is failed.
	/// </summary>
	public StateAction actionOnQuestFailed => _actionOnQuestFailed;
}
