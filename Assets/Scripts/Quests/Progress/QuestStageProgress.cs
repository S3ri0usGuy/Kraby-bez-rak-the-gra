/// <summary>
/// A class that shows the progress of the quest objective.
/// </summary>
public class QuestStageProgress
{
	/// <summary>
	/// Gets the stage.
	/// </summary>
	public QuestStage stage { get; }

	/// <summary>
	/// Gets/sets the state of the quest stage.
	/// </summary>
	public QuestStageState state { get; set; }

	public QuestStageProgress(QuestStage stage, QuestStageState state)
	{
		this.stage = stage;
		this.state = state;
	}
}
