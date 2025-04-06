using UnityEngine;
using UnityEngine.Localization.Components;

/// <summary>
/// A component that represents a <see cref="QuestListView" /> quest stage item.
/// </summary>
public class QuestStageListItem : MonoBehaviour
{
	[SerializeField]
	private GameObject _cross;
	[SerializeField]
	private GameObject _checkmark;
	[SerializeField]
	private LocalizeStringEvent _localizeEvent;

	/// <summary>
	/// Gets the quest stage that is bound to this item.
	/// </summary>
	public QuestStage stage { get; private set; }

	private void UpdateState(QuestStageState state)
	{
		(bool crossActive, bool checkmarkActive) = state switch
		{
			QuestStageState.Completed => (false, true),
			QuestStageState.Failed => (true, false),
			_ => (false, false)
		};

		_cross.SetActive(crossActive);
		_checkmark.SetActive(checkmarkActive);
	}

	/// <summary>
	/// Binds this list item to the quest stage.
	/// </summary>
	/// <param name="stage">The quest stage to bind this item to.</param>
	public void Bind(QuestStage stage)
	{
		if (this.stage)
		{
			throw new System.InvalidOperationException("Tried to bind the quest stage list item more than once.");
		}
		this.stage = stage;

		var system = QuestSystem.instance;
		system.questStageUpdated += OnQuestStageUpdated;
		UpdateState(system.GetStageState(stage));

		_localizeEvent.StringReference = stage.description;
	}

	private void OnQuestStageUpdated(QuestSystem questSystem, QuestStageUpdatedEventArgs e)
	{
		UpdateState(e.newStageState);
	}
}
