using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// A class that represents the subquest (a task inside the quest).
/// </summary>
[CreateAssetMenu(fileName = "Subquest", menuName = "Quests/Subquest")]
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
	[Tooltip("If checked, the subquest will not be visible. " +
		"This setting doesn't matter if the assigned quest is not visible.")]
	private bool _hidden = false;
	[SerializeField]
	[Tooltip("Defines the order of the subquest in the UI, relative to the assigned quest. " +
		"The lesser the value, the higher the subquest.")]
	private int _order = 0;

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
	/// Gets a flag indicating whether this subquest is hidden.
	/// </summary>
	/// <remarks>
	/// Note that this property doesn't matter if the <see cref="quest" /> is
	/// hidden.
	/// </remarks>
	public bool hidden => _hidden;

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

	/// <summary>
	/// Defines the order of the subquest in the UI, relative to the assigned quest. 
	/// The lesser the value, the higher the subquest.
	/// </summary>
	public int order => _order;
}
