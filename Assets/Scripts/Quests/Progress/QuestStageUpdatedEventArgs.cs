public class QuestStageUpdatedEventArgs
{
	public Quest quest { get; }
	public QuestStageState oldQuestState { get; }
	public QuestStageState newQuestState { get; }

	public QuestStageUpdatedEventArgs(Quest quest, QuestStageState oldQuestState, QuestStageState newQuestState)
	{
		this.quest = quest;
		this.oldQuestState = oldQuestState;
		this.newQuestState = newQuestState;
	}
}
