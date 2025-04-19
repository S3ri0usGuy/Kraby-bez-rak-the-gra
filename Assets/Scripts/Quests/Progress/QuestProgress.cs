using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that stores the quest progress.
/// </summary>
public class QuestProgress
{
	private readonly Dictionary<Subquest, SubquestState> _subquests;

	/// <summary>
	/// Gets the quest reference.
	/// </summary>
	public Quest quest { get; }

	/// <summary>
	/// Gets/sets a state of the quest.
	/// </summary>
	public QuestState state { get; set; }

	/// <summary>
	/// Gets a dictionary that maps the subquests to their states.
	/// </summary>
	public Dictionary<Subquest, SubquestState> subquests => _subquests;

	public QuestProgress(Quest quest, QuestState state)
	{
		this.quest = quest;
		this.state = state;

		_subquests = new();
	}

	/// <summary>
	/// Gets the subquest state.
	/// </summary>
	/// <param name="subquest">A subquest of the quest.</param>
	/// <returns>
	/// A state of the <paramref name="subquest"/>.
	/// </returns>
	/// <exception cref="System.ArgumentException">Thrown when the subquest is invalid for this quest.</exception>
	public SubquestState GetSubquestState(Subquest subquest)
	{
		if (subquest.quest != quest)
		{
			throw new System.ArgumentException(
				"Attempted to get a state of the subquest that doesn't belong to the quest. " +
				$"Quest name: \"{quest.name}\", the subquest name: {subquest.quest.name}.");
		}

		return subquests.GetValueOrDefault(subquest, SubquestState.None);
	}

	/// <summary>
	/// Sets the subquest state to a specific value.
	/// </summary>
	/// <param name="subquest">A subquest of the quest.</param>
	/// <param name="state">A state of the subquest.</param>
	/// <exception cref="System.ArgumentException">Thrown when the subquest is invalid for this quest.</exception>
	public void SetSubquestState(Subquest subquest, SubquestState state)
	{
		if (subquest.quest != quest)
		{
			throw new System.ArgumentException(
				"Attempted to set a state of the subquest that doesn't belong to the quest. " +
				$"Quest name: \"{quest.name}\", the subquest name: {subquest.quest.name}.");
		}

		if (subquests.TryGetValue(subquest, out var previousState) &&
			previousState != SubquestState.Active)
		{
			Debug.LogWarning($"Suspicious subquest (\"{subquest.name}\") state change was detected: {previousState} -> {state}.", subquest);
		}

		subquests[subquest] = state;
	}
}
