public class QuestStageUpdatedEventArgs
{
	public Quest quest { get; }
	public QuestStageState oldStageState { get; }
	public QuestStageState newStageState { get; }

	public QuestStageUpdatedEventArgs(Quest quest, QuestStageState oldStageState, QuestStageState newStageState)
	{
		this.quest = quest;
		this.oldStageState = oldStageState;
		this.newStageState = newStageState;
	}
}
