using System.Linq;
using UnityEditor;
using UnityEngine;

public class DialogueSaveEventsWindow : EditorWindow
{
	private Vector2 _scrollPosition;

	[MenuItem("Game Design/Dialogue Save Events View")]
	public static void ShowWindow()
	{
		GetWindow<DialogueSaveEventsWindow>("Dialogue Save Events View");
	}

	private void OnGUI()
	{
		if (!DialogueSaveEventsProvider.exists)
		{
			EditorGUILayout.HelpBox("Dialogue Save Events Provider not initialized.", MessageType.Info);
			return;
		}

		_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

		var events = DialogueSaveEventsProvider.instance.savedEvents;

		if (!events.Any())
		{
			EditorGUILayout.HelpBox("There are no saved dialogue events.", MessageType.Info);
		}

		foreach (var eventKey in events)
		{
			EditorGUILayout.LabelField(eventKey);
		}

		EditorGUILayout.EndScrollView();
	}
}
