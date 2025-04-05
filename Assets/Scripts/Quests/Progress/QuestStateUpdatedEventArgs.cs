public class QuestStateUpdatedEventArgs
{
	public Quest quest { get; }
	public QuestState oldQuestState { get; }
	public QuestState newQuestState { get; }

	public QuestStateUpdatedEventArgs(Quest quest, QuestState oldQuestState, QuestState newQuestState)
	{
		this.quest = quest;
		this.oldQuestState = oldQuestState;
		this.newQuestState = newQuestState;
	}
}
