using UnityEngine;

/// <summary>
/// A component that fails quests when the in-game time is over.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(QuestSystem))]
public class QuestFailOnTimeOver : MonoBehaviour
{
	private QuestSystem _questSystem;

	private void Awake()
	{
		_questSystem = GetComponent<QuestSystem>();

		if (Clock.exists)
		{
			Clock.instance.timeOver += OnTimeOver;
		}
	}

	private void OnTimeOver(Clock clock)
	{
		foreach (var quest in _questSystem.GetQuests())
		{
			if (!quest.failOnTimeOver) continue;
			foreach (var stage in _questSystem.GetQuestStages(quest))
			{
				if (_questSystem.GetStageState(stage) == QuestStageState.Active)
				{
					_questSystem.SetStageState(stage, QuestStageState.Failed);
				}
			}
			_questSystem.SetQuestState(quest, QuestState.Failed);
		}
	}
}
