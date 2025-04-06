public class QuestStageUpdatedEventArgs
{
	public QuestStage stage { get; }
	public QuestStageState oldStageState { get; }
	public QuestStageState newStageState { get; }

	public Quest quest => stage.quest;

	public QuestStageUpdatedEventArgs(QuestStage stage, QuestStageState oldStageState, QuestStageState newStageState)
	{
		this.stage = stage;
		this.oldStageState = oldStageState;
		this.newStageState = newStageState;
	}
}
