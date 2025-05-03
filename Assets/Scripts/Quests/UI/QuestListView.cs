using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// A component that displays the quests in a "nested" list.
/// </summary>
public class QuestListView : MonoBehaviour
{
	private class QuestItemsGroup
	{
		public QuestListItem questItem { get; }
		public List<SubquestListItem> subquestItems { get; private set; }

		public QuestItemsGroup(QuestListItem questItem)
		{
			this.questItem = questItem;
			subquestItems = new();
		}

		public void SortItems()
		{
			subquestItems = subquestItems.OrderBy(x => x.subquest.order).ToList();
		}
	}

	private List<QuestItemsGroup> _questGroups = new();

	[SerializeField]
	private QuestListItem _questItemPrefab;
	[SerializeField]
	[FormerlySerializedAs("_questStageItemPrefab")]
	private SubquestListItem _subquestItemPrefab;

	private void Start()
	{
		if (!QuestSystem.exists) return;

		var questSystem = QuestSystem.instance;
		foreach (var quest in questSystem.GetQuests())
		{
			AddQuest(quest);
			foreach (var subquest in questSystem.GetQuestSubquests(quest))
			{
				AddSubquest(subquest);
			}
		}

		questSystem.questStarted += OnQuestStarted;
		questSystem.subquestStateUpdated += OnSubquestUpdated;
	}

	private void OnQuestStarted(QuestSystem questSystem, Quest quest)
	{
		AddQuest(quest);
	}

	private void OnSubquestUpdated(QuestSystem questSystem, SubquestStateUpdatedEventArgs e)
	{
		AddSubquest(e.subquest);
	}

	private void Reorder()
	{
		int index = 0;
		foreach (var group in _questGroups)
		{
			group.questItem.transform.SetSiblingIndex(index++);
			foreach (var subquestItem in group.subquestItems)
			{
				subquestItem.transform.SetSiblingIndex(index++);
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
	}

	private QuestItemsGroup AddQuest(Quest quest)
	{
		if (quest.hidden) return null;

		var group = _questGroups.Find(x => x.questItem.quest == quest);
		if (group == null)
		{
			QuestListItem questItem = Instantiate(_questItemPrefab, transform);
			questItem.Bind(quest);

			QuestItemsGroup newGroup = new(questItem);
			_questGroups.Add(newGroup);
			_questGroups = _questGroups.OrderBy(x => x.questItem.quest.order).ToList();

			Reorder();

			return newGroup;
		}

		return group;
	}

	private void AddSubquest(Subquest subquest)
	{
		if (subquest.hidden) return;

		var group = _questGroups.Find(x => x.questItem.quest == subquest.quest) ?? AddQuest(subquest.quest);
		if (group == null) return;

		if (group.subquestItems.FindIndex(x => x.subquest == subquest) == -1)
		{
			SubquestListItem subquestItem = Instantiate(_subquestItemPrefab, transform);
			subquestItem.Bind(subquest);

			group.subquestItems.Add(subquestItem);
			group.SortItems();

			Reorder();
		}
	}
}
