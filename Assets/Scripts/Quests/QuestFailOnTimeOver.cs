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
			foreach (var subquest in _questSystem.GetQuestSubquests(quest))
			{
				if (_questSystem.GetSubquestState(subquest) == SubquestState.Active)
				{
					_questSystem.SetSubquestState(subquest, SubquestState.Failed);
				}
			}
			_questSystem.SetQuestState(quest, QuestState.Failed);
		}
	}
}
