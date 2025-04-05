using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that stores the quest progress.
/// </summary>
public class QuestProgress
{
	private readonly Dictionary<QuestStage, QuestStageState> _stages;

	/// <summary>
	/// Gets the quest reference.
	/// </summary>
	public Quest quest { get; }

	/// <summary>
	/// Gets/sets a state of the quest.
	/// </summary>
	public QuestState state { get; set; }

	/// <summary>
	/// Gets a dictionary that maps the quest stages to their states.
	/// </summary>
	public Dictionary<QuestStage, QuestStageState> stages => _stages;

	public QuestProgress(Quest quest, QuestState state)
	{
		this.quest = quest;
		this.state = state;

		_stages = new();
	}

	/// <summary>
	/// Gets the stage state.
	/// </summary>
	/// <param name="stage">A stage of the quest.</param>
	/// <returns>
	/// A state of the <paramref name="stage"/>.
	/// </returns>
	public QuestStageState GetStage(QuestStage stage)
	{
		if (stage.quest != quest)
		{
			throw new System.ArgumentException(
				"Attempted to get a state of the stage that doesn't belong to the quest. " +
				$"Quest name: \"{quest.name}\", the stage quest name: {stage.quest.name}.");
		}

		return stages.GetValueOrDefault(stage, QuestStageState.None);
	}

	/// <summary>
	/// Sets the stage state to a specific value.
	/// </summary>
	/// <param name="stage">A stage of the quest.</param>
	/// <param name="state">A state of the quest stage</param>
	public void SetStage(QuestStage stage, QuestStageState state)
	{
		if (stage.quest != quest)
		{
			throw new System.ArgumentException(
				"Attempted to set a state of the stage that doesn't belong to the quest. " +
				$"Quest name: \"{quest.name}\", the stage quest name: {stage.quest.name}.");
		}

		if (stages.TryGetValue(stage, out var previousState) &&
			previousState != QuestStageState.Active)
		{
			Debug.LogWarning($"Suspicious quest stage (\"{stage.name}\") state change was detected: {previousState} -> {state}.", stage);
		}

		stages[stage] = state;
	}
}
