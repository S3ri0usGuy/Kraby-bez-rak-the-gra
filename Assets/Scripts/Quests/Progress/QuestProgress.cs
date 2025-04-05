using System.Collections.Generic;

/// <summary>
/// A class that stores the quest progress.
/// </summary>
public class QuestProgress
{
	/// <summary>
	/// Gets the quest reference.
	/// </summary>
	public Quest quest { get; }

	/// <summary>
	/// Gets a state of the quest.
	/// </summary>
	public QuestState state { get; }

	/// <summary>
	/// Gets a readonly dictionary that maps the quest stages to their states.
	/// </summary>
	public IReadOnlyDictionary<QuestStage, QuestStageState> stages { get; }

	public QuestProgress(Quest quest, QuestState state, IReadOnlyDictionary<QuestStage, QuestStageState> stages)
	{
		this.quest = quest;
		this.state = state;
		this.stages = stages;
	}
}
