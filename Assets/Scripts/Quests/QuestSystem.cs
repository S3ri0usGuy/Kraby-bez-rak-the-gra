using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSystem : SingletonMonoBehaviour<QuestSystem>
{
	private Dictionary<Quest, QuestProgress> _quests = new();

	public delegate void QuestAction(QuestSystem questSystem, Quest quest);
	public delegate void QuestStateUpdatedAction(QuestSystem questSystem, QuestStateUpdatedEventArgs e);
	public delegate void QuestStageUpdatedAction(QuestSystem questSystem, QuestStageUpdatedEventArgs e);

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
	/// Not to be confused with <see cref="questStageUpdated" />.
	/// </remarks>
	public event QuestStateUpdatedAction questStateUpdated;
	/// <summary>
	/// Event that is triggered when the stage state of one of the quests is updated.
	/// </summary>
	/// <remarks>
	/// Not to be confused with <see cref="questStateUpdated" />.
	/// </remarks>
	public event QuestStageUpdatedAction questStageUpdated;

	/// <summary>
	/// Loads the quests progress.
	/// </summary>
	/// <param name="quests">A dictionary containing the quest progresses.</param>
	public void LoadQuests(Dictionary<Quest, QuestProgress> quests)
	{
		_quests = quests;
	}

	/// <summary>
	/// Gets an enumerable containing all active, completed and failed quests.
	/// </summary>
	/// <returns>An enumerable containing all active, completed and failed quests.</returns>
	public IEnumerable<Quest> GetQuests()
	{
		return _quests.Keys;
	}

	/// <summary>
	/// Gets an enumerable containing all active, completed and failed stages of the quest.
	/// </summary>
	/// <returns>
	/// An enumerable containing all active, completed and failed stages of the quest.
	/// Empty if the quest was not found or if it has no stages.
	/// </returns>
	/// <exception cref="System.ArgumentNullException" />
	public IEnumerable<QuestStage> GetQuestStages(Quest quest)
	{
		if (!quest)
		{
			throw new System.ArgumentNullException(nameof(quest));
		}

		if (_quests.TryGetValue(quest, out var progress))
		{
			return progress.stages.Keys;
		}
		return Enumerable.Empty<QuestStage>();
	}

	/// <summary>
	/// Gets the state of the quest.
	/// </summary>
	/// <param name="quest">The quest to get the stage for.</param>
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

	/// <summary>
	/// Sets the quest state to a new value.
	/// </summary>
	/// <param name="quest">The quest which state is set.</param>
	/// <param name="state">The new quest state value.</param>
	/// <exception cref="System.ArgumentNullException" />
	/// <exception cref="System.Argument" />
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
			List<(QuestStage, QuestStageState)> valuesToSet = new();
			foreach (var (stage, stageState) in progress.stages)
			{
				if (stageState != QuestStageState.Active)
					continue;

				var action = state == QuestState.Completed ? stage.actionOnQuestPassed : stage.actionOnQuestFailed;

				if (action == QuestStage.StateAction.None)
					continue;

				var newStageState = action == QuestStage.StateAction.Pass ? QuestStageState.Completed : QuestStageState.Failed;
				valuesToSet.Add((stage, newStageState));
			}
			// Prevents the "Collection was modified" error
			foreach (var (stage, stageState) in valuesToSet)
			{
				SetStageState(stage, stageState);
			}
		}

		progress.state = state;

		QuestStateUpdatedEventArgs eventArgs = new(quest, oldState, state);
		questStateUpdated?.Invoke(this, eventArgs);
	}

	/// <summary>
	/// Gets the state of the given stage.
	/// </summary>
	/// <param name="stage"></param>
	/// <returns>
	/// A state of the given stage. <see cref="QuestStageState.None" /> is returned
	/// when the stage or its quest has not been encountered.
	/// </returns>
	public QuestStageState GetStageState(QuestStage stage)
	{
		if (!stage.quest)
		{
			Debug.LogError($"The quest stage (\"{stage.name}\") has no quest assigned to it.", stage);
			return QuestStageState.None;
		}

		if (_quests.TryGetValue(stage.quest, out var progress))
		{
			return progress.GetStage(stage);
		}

		return QuestStageState.None;
	}

	/// <summary>
	/// Sets the stage to a new state.
	/// </summary>
	/// <param name="stage">The stage to manipulate.</param>
	/// <param name="state">The state to set.</param>
	/// <exception cref="System.ArgumentNullException" />
	/// <exception cref="System.ArgumentException" />
	public void SetStageState(QuestStage stage, QuestStageState state)
	{
		if (!stage)
		{
			throw new System.ArgumentNullException(nameof(stage));
		}
		if (state == QuestStageState.None)
		{
			throw new System.ArgumentException("Cannot set the stage state to \"None\".", nameof(state));
		}

		Quest quest = stage.quest;
		if (!quest)
		{
			Debug.LogError($"The quest stage (\"{stage.name}\") has no quest assigned to it.", stage);
			return;
		}

		if (_quests.TryGetValue(quest, out var questProgress))
		{
			if (questProgress.state != QuestState.Active)
			{
				Debug.LogWarning($"A stage of the non-active quest (\"{quest.name}\") was modified.");
			}
		}
		else
		{
			// Create the new quest progress if it wasn't already created
			SetQuestState(quest, QuestState.Active);
			questProgress = _quests[quest];
		}

		QuestStageState oldState = questProgress.GetStage(stage);
		questProgress.SetStage(stage, state);

		QuestStageUpdatedEventArgs eventArgs = new(stage, oldState, state);
		questStageUpdated?.Invoke(this, eventArgs);
	}
}
