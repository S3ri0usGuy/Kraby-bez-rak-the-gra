using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSystem : SingletonMonoBehaviour<QuestSystem>
{
	private readonly Dictionary<Quest, QuestProgress> _quests = new();

	public delegate void QuestAction(QuestSystem questSystem, Quest quest);
	public delegate void QuestStateUpdatedAction(QuestSystem questSystem, QuestStateUpdatedEventArgs e);
	public delegate void SubquestStateUpdatedAction(QuestSystem questSystem, SubquestStateUpdatedEventArgs e);

	/// <summary>
	/// Event that is triggered when the new quest is started.
	/// </summary>
	/// <remarks>
	/// Called before the <see cref="questStateUpdated" />.
	/// </remarks>
	public event QuestAction questStarted;
	/// <summary>
	/// Event that is triggered when the general quest state is updated.
	/// </summary>
	/// <remarks>
	/// Not to be confused with <see cref="subquestStateUpdated" />.
	/// </remarks>
	public event QuestStateUpdatedAction questStateUpdated;
	/// <summary>
	/// Event that is triggered when the subquest state of one of the quests is updated.
	/// </summary>
	/// <remarks>
	/// Not to be confused with <see cref="questStateUpdated" />.
	/// </remarks>
	public event SubquestStateUpdatedAction subquestStateUpdated;

	/// <summary>
	/// Gets an enumerable containing all active, completed and failed quests.
	/// </summary>
	/// <returns>An enumerable containing all active, completed and failed quests.</returns>
	public IEnumerable<Quest> GetQuests()
	{
		return _quests.Keys;
	}

	/// <summary>
	/// Gets an enumerable containing all active, completed and failed subquests of the quest.
	/// </summary>
	/// <returns>
	/// An enumerable containing all active, completed and failed subquests of the quest.
	/// Empty if the quest was not found or if it has no subquests.
	/// </returns>
	/// <exception cref="System.ArgumentNullException" />
	public IEnumerable<Subquest> GetQuestSubquests(Quest quest)
	{
		if (!quest)
		{
			throw new System.ArgumentNullException(nameof(quest));
		}

		if (_quests.TryGetValue(quest, out var progress))
		{
			return progress.subquests.Keys;
		}
		return Enumerable.Empty<Subquest>();
	}

	/// <summary>
	/// Gets the state of the quest.
	/// </summary>
	/// <param name="quest">The quest to get the state for.</param>
	/// <returns>
	/// The state of the quest or <see cref="QuestState.None" />, if the quest hasn't started yet.
	/// </returns>
	public QuestState GetQuestState(Quest quest)
	{
		if (_quests.TryGetValue(quest, out var progress))
		{
			return progress.state;
		}

		return QuestState.None;
	}

	private void CascadeSubquests(QuestProgress progress, QuestState state)
	{
		List<(Subquest, SubquestState)> valuesToSet = new();
		foreach (var (subquest, subquestState) in progress.subquests)
		{
			if (subquestState != SubquestState.Active)
				continue;

			var action = state == QuestState.Completed ? subquest.actionOnQuestPassed : subquest.actionOnQuestFailed;

			if (action == Subquest.StateAction.None)
				continue;

			var newSubquestState = action == Subquest.StateAction.Pass ? SubquestState.Completed : SubquestState.Failed;
			valuesToSet.Add((subquest, newSubquestState));
		}
		// Prevents the "Collection was modified" error
		foreach (var (subquest, subquestState) in valuesToSet)
		{
			SetSubquestState(subquest, subquestState);
		}
	}

	/// <summary>
	/// Sets the quest state to a new value.
	/// </summary>
	/// <param name="quest">The quest which state is set (<see cref="QuestState.None"/> is invalid).</param>
	/// <param name="state">The new quest state value.</param>
	/// <exception cref="System.ArgumentNullException" />
	/// <exception cref="System.ArgumentException">
	/// Attempted to set an invalid quest state.
	/// </exception>
	public void SetQuestState(Quest quest, QuestState state)
	{
		if (!quest)
		{
			throw new System.ArgumentNullException(nameof(quest));
		}
		if (state == QuestState.None)
		{
			throw new System.ArgumentException("Cannot set the quest state to \"None\".", nameof(state));
		}
		if (!System.Enum.IsDefined(typeof(QuestState), state))
		{
			throw new System.ArgumentException($"The quest state \"{state}\" is invalid.", nameof(state));
		}

		QuestState oldState = QuestState.None;
		if (_quests.TryGetValue(quest, out var progress))
		{
			if (progress.state != QuestState.Active)
			{
				Debug.LogWarning($"Suspicious quest (\"{quest.name}\") state change " +
					$"was detected: {progress.state} -> {state}.", quest);
			}

			oldState = progress.state;
		}
		else
		{
			// Set the state to None by default to prevent warnings
			progress = _quests[quest] = new QuestProgress(quest, QuestState.None);
			questStarted?.Invoke(this, quest);
		}

		if (state == QuestState.Completed || state == QuestState.Failed)
		{
			CascadeSubquests(progress, state);
		}

		progress.state = state;

		QuestStateUpdatedEventArgs eventArgs = new(quest, oldState, state);
		questStateUpdated?.Invoke(this, eventArgs);
	}

	/// <summary>
	/// Gets the state of the given subquest.
	/// </summary>
	/// <param name="subquest">A subquest for which the state is needed to be found.</param>
	/// <returns>
	/// A state of the given subquest. <see cref="SubquestState.None" /> is returned
	/// when the subquest or its quest has not been encountered.
	/// </returns>
	public SubquestState GetSubquestState(Subquest subquest)
	{
		if (!subquest.quest)
		{
			Debug.LogError($"The subquest (\"{subquest.name}\") has no quest assigned to it.", subquest);
			return SubquestState.None;
		}

		if (_quests.TryGetValue(subquest.quest, out var progress))
		{
			return progress.GetSubquestState(subquest);
		}

		return SubquestState.None;
	}

	/// <summary>
	/// Sets the subquest state.
	/// </summary>
	/// <param name="subquest">The subquest to manipulate.</param>
	/// <param name="state">The state to set (<see cref="SubquestState.None"/> is invalid).</param>
	/// <exception cref="System.ArgumentNullException" />
	/// <exception cref="System.ArgumentException">
	/// Attempted to set an invalid quest state.
	/// </exception>
	public void SetSubquestState(Subquest subquest, SubquestState state)
	{
		if (!subquest)
		{
			throw new System.ArgumentNullException(nameof(subquest));
		}
		if (state == SubquestState.None)
		{
			throw new System.ArgumentException("Cannot set the subquest state to \"None\".", nameof(state));
		}
		if (!System.Enum.IsDefined(typeof(SubquestState), state))
		{
			throw new System.ArgumentException($"The subquest state \"{state}\" is invalid.", nameof(state));
		}

		Quest quest = subquest.quest;
		if (!quest)
		{
			Debug.LogError($"The subquest (\"{subquest.name}\") has no quest assigned to it.", subquest);
			return;
		}

		if (_quests.TryGetValue(quest, out var questProgress))
		{
			if (questProgress.state != QuestState.Active)
			{
				Debug.LogWarning($"A subquest (\"{subquest.name}\") of the non-active quest (\"{quest.name}\") was modified.");
			}
		}
		else
		{
			// Create the new quest progress if it wasn't already created
			SetQuestState(quest, QuestState.Active);
			questProgress = _quests[quest];
		}

		SubquestState oldState = questProgress.GetSubquestState(subquest);
		questProgress.SetSubquestState(subquest, state);

		SubquestStateUpdatedEventArgs eventArgs = new(subquest, oldState, state);
		subquestStateUpdated?.Invoke(this, eventArgs);
	}
}
