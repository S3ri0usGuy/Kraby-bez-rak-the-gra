using System.IO;
using UnityEditor;

/// <summary>
/// A class which automatically populates dialogue groups with
/// dialogues located in the same root folder.
/// </summary>
public class AutoDialogueLoader : AssetPostprocessor
{
	// Flag that prevents infinite loops
	private static bool _isProcessing = false;

	private static bool ContainsPath(string[] array, string path)
	{
		foreach (var item in array)
		{
			if (item.ToLower().Contains(path))
				return true;
		}
		return false;
	}

	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		if (_isProcessing) return;
		_isProcessing = true;

		var collections = AssetDatabase.FindAssets($"t:{nameof(DialogueGroup)}");
		bool save = false;
		foreach (string guid in collections)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			string directory = Path.GetDirectoryName(path).ToLower().Replace('\\', '/');
			if (ContainsPath(importedAssets, directory) || ContainsPath(deletedAssets, directory) ||
				ContainsPath(movedAssets, directory) || ContainsPath(movedFromAssetPaths, directory))
			{
				var collection = AssetDatabase.LoadAssetAtPath<DialogueGroup>(path);
				collection.LoadNodes();
				save = true;
			}
		}
		if (save) AssetDatabase.SaveAssets();

		_isProcessing = false;
	}
}
