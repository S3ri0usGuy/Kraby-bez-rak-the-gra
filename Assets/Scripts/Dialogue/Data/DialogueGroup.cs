using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A scriptable object which holds all dialogue nodes in the
/// same folder (including subfolders) it's located.
/// </summary>
[CreateAssetMenu(fileName = "Dialogue Group", menuName = "Dialogue/Group")]
public class DialogueGroup : ScriptableObject
{
	[SerializeField]
	[Tooltip("Dialogue nodes that were found. " +
		"Don't change this list manually because you will lose changes.")]
	private DialogueNode[] _dialogues = { };

	/// <summary>
	/// Gets a collection of dialogue nodes associated with this group.
	/// </summary>
	public IReadOnlyList<DialogueNode> dialogues => _dialogues;

#if UNITY_EDITOR
	public void LoadNodes()
	{
		string assetPath = AssetDatabase.GetAssetPath(this);
		string assetDirectory = Path.GetDirectoryName(assetPath);

		List<DialogueNode> loadedNodes = new();
		var files = Directory.GetFiles(assetDirectory, "*.asset", SearchOption.AllDirectories);
		foreach (var file in files)
		{
			var node = AssetDatabase.LoadAssetAtPath<DialogueNode>(file);
			if (node == null) continue;

			loadedNodes.Add(node);
		}
		var newNodes = loadedNodes.ToArray();
		// Check if the array was actually updated
		if (!ArrayUtility.ArrayEquals(_dialogues, newNodes))
		{
			EditorUtility.SetDirty(this);
			_dialogues = newNodes;
		}
	}
#endif
}
