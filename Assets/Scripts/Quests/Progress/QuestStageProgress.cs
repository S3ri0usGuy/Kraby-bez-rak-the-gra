using UnityEngine;

/// <summary>
/// A class that shows the progress of the quest objective.
/// </summary>
public class QuestStageProgress : MonoBehaviour
{
	/// <summary>
	/// Gets the stage.
	/// </summary>
	public QuestStage stage { get; }

	/// <summary>
	/// Gets the state of the quest stage.
	/// </summary>
	public QuestStageState state { get; }
}
