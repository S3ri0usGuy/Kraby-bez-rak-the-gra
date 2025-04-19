using UnityEngine;
using UnityEngine.Localization.Components;

/// <summary>
/// A component that represents a <see cref="QuestListView" /> subquest item.
/// </summary>
public class SubquestListItem : MonoBehaviour
{
	[SerializeField]
	private GameObject _cross;
	[SerializeField]
	private GameObject _checkmark;
	[SerializeField]
	private LocalizeStringEvent _localizeEvent;

	/// <summary>
	/// Gets the subquest that is bound to this item.
	/// </summary>
	public Subquest subquest { get; private set; }

	private void UpdateState(SubquestState state)
	{
		(bool crossActive, bool checkmarkActive) = state switch
		{
			SubquestState.Completed => (false, true),
			SubquestState.Failed => (true, false),
			_ => (false, false)
		};

		_cross.SetActive(crossActive);
		_checkmark.SetActive(checkmarkActive);
	}

	/// <summary>
	/// Binds this list item to the subquest.
	/// </summary>
	/// <param name="subquest">The subquest to bind this item to.</param>
	public void Bind(Subquest subquest)
	{
		if (this.subquest)
		{
			throw new System.InvalidOperationException("Tried to bind the quest subquest list item more than once.");
		}
		this.subquest = subquest;

		var system = QuestSystem.instance;
		system.subquestStateUpdated += OnQuestSubquestUpdated;
		UpdateState(system.GetSubquestState(subquest));

		_localizeEvent.StringReference = subquest.description;
	}

	private void OnQuestSubquestUpdated(QuestSystem questSystem, SubquestStateUpdatedEventArgs e)
	{
		if (e.subquest == subquest) UpdateState(e.newSubquestState);
	}
}
