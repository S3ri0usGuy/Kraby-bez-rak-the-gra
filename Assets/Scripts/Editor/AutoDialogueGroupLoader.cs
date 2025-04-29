using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A class which automatically populates scriptable object groups with
/// objects located in the same root folder.
/// </summary>
public class ScriptableObjectGroupAutoLoader : AssetPostprocessor
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

	private static bool ProcessType(Type type, string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		var collections = AssetDatabase.FindAssets($"t:{type.Name}");
		bool save = false;
		foreach (string guid in collections)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			string directory = Path.GetDirectoryName(path).ToLower().Replace('\\', '/');
			if (ContainsPath(importedAssets, directory) || ContainsPath(deletedAssets, directory) ||
				ContainsPath(movedAssets, directory) || ContainsPath(movedFromAssetPaths, directory))
			{
				var asset = AssetDatabase.LoadAssetAtPath(path, type);
				if (asset is IScriptableObjectGroup group)
				{
					group.LoadObjects();
				}
				save = true;
			}
		}
		return save;
	}

	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		if (_isProcessing) return;
		_isProcessing = true;

		bool save = false;

		var scriptableType = typeof(ScriptableObject);
		var groupType = typeof(IScriptableObjectGroup);
		// Get all types that implement the IScriptableObjectGroup interface and are scriptable objects
		var types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => scriptableType.IsAssignableFrom(p) && groupType.IsAssignableFrom(p));

		foreach (var type in types)
		{
			save |= ProcessType(type, importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
		}

		if (save) AssetDatabase.SaveAssets();

		_isProcessing = false;
	}
}
