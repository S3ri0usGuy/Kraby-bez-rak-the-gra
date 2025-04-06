using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A component that displays the quests in a "nested" list.
/// </summary>
public class QuestListView : MonoBehaviour
{
	private class QuestItemsGroup
	{
		public QuestListItem questItem { get; }
		public List<QuestStageListItem> stageItems { get; }

		public QuestItemsGroup(QuestListItem questItem)
		{
			this.questItem = questItem;
			stageItems = new();
		}
	}

	private readonly List<QuestItemsGroup> _questGroups = new();

	[SerializeField]
	private QuestListItem _questItemPrefab;
	[SerializeField]
	private QuestStageListItem _questStageItemPrefab;

	private void Start()
	{
		var questSystem = QuestSystem.instance;
		foreach (var quest in questSystem.GetQuests())
		{
			AddQuest(quest);
			foreach (var stage in questSystem.GetQuestStages(quest))
			{
				AddStage(stage);
			}
		}

		questSystem.questStarted += OnQuestStarted;
		questSystem.questStageUpdated += OnStageUpdated;
	}

	private void OnQuestStarted(QuestSystem questSystem, Quest quest)
	{
		AddQuest(quest);
	}

	private void OnStageUpdated(QuestSystem questSystem, QuestStageUpdatedEventArgs e)
	{
		AddStage(e.stage);
	}

	private void Reorder()
	{
		int index = 0;
		foreach (var group in _questGroups)
		{
			group.questItem.transform.SetSiblingIndex(index++);
			foreach (var stageItem in group.stageItems)
			{
				stageItem.transform.SetSiblingIndex(index++);
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
	}

	private QuestItemsGroup AddQuest(Quest quest)
	{
		var group = _questGroups.Find(x => x.questItem.quest == quest);
		if (group == null)
		{
			QuestListItem questItem = Instantiate(_questItemPrefab, transform);
			questItem.Bind(quest);

			QuestItemsGroup newGroup = new(questItem);
			_questGroups.Add(newGroup);
			Reorder();

			return newGroup;
		}

		return group;
	}

	private void AddStage(QuestStage stage)
	{
		var group = _questGroups.Find(x => x.questItem.quest == stage.quest) ?? AddQuest(stage.quest);
		if (group.stageItems.FindIndex(x => x.stage == stage) == -1)
		{
			QuestStageListItem questStageItem = Instantiate(_questStageItemPrefab, transform);
			questStageItem.Bind(stage);

			group.stageItems.Add(questStageItem);
			Reorder();
		}
	}
}
