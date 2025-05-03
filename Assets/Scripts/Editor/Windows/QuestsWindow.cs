using System.Linq;
using UnityEditor;
using UnityEngine;

public class QuestListEditorWindow : EditorWindow
{
	private Vector2 _scrollPosition;
	private bool _showHidden = true;

	[MenuItem("Game Design/Quests Viewer")]
	public static void ShowWindow()
	{
		GetWindow<QuestListEditorWindow>("Quests Viewer");
	}

	private void OnGUI()
	{
		if (!QuestSystem.exists)
		{
			EditorGUILayout.HelpBox("Quest System not initialized.", MessageType.Info);
			return;
		}

		_showHidden = EditorGUILayout.Toggle("Show hidden", _showHidden);

		_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

		var questSystem = QuestSystem.instance;
		var quests = questSystem.GetQuests()
			.OrderBy(q => q.order)
			.ToList();

		foreach (var quest in quests)
		{
			DrawQuest(quest, questSystem);
		}

		EditorGUILayout.EndScrollView();
	}

	private void DrawQuest(Quest quest, QuestSystem questSystem)
	{
		if (!_showHidden && quest.hidden) return;

		var state = questSystem.GetQuestState(quest);
		var stateLabel = GetStateLabel(state);

		EditorGUILayout.BeginVertical("box");

		string label = quest.hidden ? "(Hidden)" : "";

		EditorGUILayout.LabelField($"[Quest] {quest.name} {stateLabel}", EditorStyles.boldLabel);

		var subquests = questSystem.GetQuestSubquests(quest)
			.OrderBy(sq => sq.order)
			.ToList();

		foreach (var subquest in subquests)
		{
			DrawSubquest(subquest, questSystem);
		}

		EditorGUILayout.EndVertical();
		EditorGUILayout.Space(4);
	}

	private void DrawSubquest(Subquest subquest, QuestSystem questSystem)
	{
		if (!_showHidden && subquest.hidden) return;

		var subState = questSystem.GetSubquestState(subquest);
		var stateLabel = GetStateLabel(subState);

		EditorGUI.indentLevel++;
		string label = subquest.hidden ? "(Hidden)" : "";
		EditorGUILayout.LabelField($"- {label} {subquest.name} {stateLabel}");
		EditorGUI.indentLevel--;
	}

	private string GetStateLabel(QuestState state)
	{
		return state switch
		{
			QuestState.Completed => "- ✓ Completed",
			QuestState.Failed => "- ✗ Failed",
			_ => "- Revealed"
		};
	}

	private string GetStateLabel(SubquestState state)
	{
		return state switch
		{
			SubquestState.Completed => "- ✓ Completed",
			SubquestState.Failed => "- ✗ Failed",
			_ => "- Revealed"
		};
	}
}
