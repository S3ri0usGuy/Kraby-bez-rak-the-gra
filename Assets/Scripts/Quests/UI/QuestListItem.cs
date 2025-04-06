using UnityEngine;
using UnityEngine.Localization.Components;

/// <summary>
/// A component that represents a <see cref="QuestListView" /> item.
/// </summary>
public class QuestListItem : MonoBehaviour
{
	[SerializeField]
	private GameObject _cross;
	[SerializeField]
	private GameObject _checkmark;
	[SerializeField]
	private LocalizeStringEvent _localizeEvent;

	/// <summary>
	/// Gets the quest that is bound to this item.
	/// </summary>
	public Quest quest { get; private set; }

	private void UpdateState(QuestState state)
	{
		(bool crossActive, bool checkmarkActive) = state switch
		{
			QuestState.Completed => (false, true),
			QuestState.Failed => (true, false),
			_ => (false, false)
		};

		_cross.SetActive(crossActive);
		_checkmark.SetActive(checkmarkActive);
	}

	/// <summary>
	/// Binds this list item to the quest.
	/// </summary>
	/// <param name="quest">The quest to bind this item to.</param>
	public void Bind(Quest quest)
	{
		if (this.quest)
		{
			throw new System.InvalidOperationException("Tried to bind the quest list item more than once.");
		}
		this.quest = quest;

		var system = QuestSystem.instance;
		system.questStateUpdated += OnQuestStateUpdated;
		UpdateState(system.GetQuestState(quest));

		_localizeEvent.StringReference = quest.questName;
	}

	private void OnQuestStateUpdated(QuestSystem questSystem, QuestStateUpdatedEventArgs e)
	{
		if (e.quest == quest) UpdateState(e.newQuestState);
	}
}
