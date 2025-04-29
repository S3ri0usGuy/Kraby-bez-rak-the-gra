using System;
using UnityEngine;

/// <summary>
/// A component that saves the <see cref="QuestSystem" /> state.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(QuestSystem))]
public class QuestSystemSaver : SavableComponent<QuestSystemSaveData>
{
	private QuestSystem _questSystem;

	[SerializeField]
	private QuestGroup _globalQuestGroup;
	[SerializeField]
	private SubquestGroup _globalSubquestGroup;

	protected override QuestSystemSaveData fallbackData => new()
	{
		quests = new(),
		subquests = new()
	};

	protected override void Awake()
	{
		_questSystem = GetComponent<QuestSystem>();

		base.Awake();
	}

	protected override void Validate(QuestSystemSaveData data)
	{
		for (int i = data.quests.Count - 1; i >= 0; i--)
		{
			var quest = data.quests[i];
			if (!_globalQuestGroup.nameToObject.TryGetValue(quest.name, out var questObject))
			{
				Debug.LogWarning($"Couldn't find a quest with the name \"{quest.name}\".", gameObject);
				data.quests.RemoveAt(i);
			}
			if (!Enum.IsDefined(typeof(QuestState), quest.state))
			{
				Debug.LogWarning($"State ({quest.state}) that is assigned to the quest \"{quest.name}\" is invalid.",
					questObject ? questObject : gameObject);
				quest.state = QuestState.None;
			}
		}
		for (int i = data.subquests.Count - 1; i >= 0; i--)
		{
			var subquest = data.subquests[i];
			if (!_globalSubquestGroup.nameToObject.TryGetValue(subquest.name, out var subquestObject))
			{
				Debug.LogWarning($"Couldn't find a subquest with the name \"{subquest.name}\".", gameObject);
				data.subquests.RemoveAt(i);
			}
			if (!Enum.IsDefined(typeof(SubquestState), subquest.state))
			{
				Debug.LogWarning($"State ({subquest.state}) that is assigned to the subquest \"{subquest.name}\" is invalid.",
					subquestObject ? subquestObject : gameObject);
				subquest.state = SubquestState.None;
			}
		}
	}

	protected override void OnLoad()
	{
		foreach (var subquest in data.subquests)
		{
			// Not allowed by the quest system
			if (subquest.state == SubquestState.None) continue;

			if (_globalSubquestGroup.nameToObject.TryGetValue(subquest.name, out var subquestObject))
			{
				_questSystem.SetSubquestState(subquestObject, subquest.state);
			}
		}

		foreach (var quest in data.quests)
		{
			// Not allowed by the quest system
			if (quest.state == QuestState.None) continue;

			if (_globalQuestGroup.nameToObject.TryGetValue(quest.name, out var questObject))
			{
				_questSystem.SetQuestState(questObject, quest.state);
			}
		}
	}

	protected override void OnSave()
	{
		data.quests.Clear();
		data.subquests.Clear();

		foreach (var quest in _questSystem.GetQuests())
		{
			QuestSaveData questSaveData = new()
			{
				name = quest.name,
				state = _questSystem.GetQuestState(quest)
			};
			data.quests.Add(questSaveData);

			foreach (var subquest in _questSystem.GetQuestSubquests(quest))
			{
				SubquestSaveData subquestSaveData = new()
				{
					name = subquest.name,
					state = _questSystem.GetSubquestState(subquest)
				};
				data.subquests.Add(subquestSaveData);
			}
		}
	}
}
