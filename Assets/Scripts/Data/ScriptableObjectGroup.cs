using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// A scriptable object base which holds other objects of the type <typeparamref name="T" /> in the
/// same folder (including subfolders) it's located.
/// </summary>
public abstract class ScriptableObjectGroup<T> : ScriptableObject, IScriptableObjectGroup
	where T : ScriptableObject
{
	private IReadOnlyDictionary<string, T> _nameToObject = null;

	[SerializeField]
	[FormerlySerializedAs("_dialogues")]
	[Tooltip("Objects that were found. " +
		"Don't change this list manually because you will lose changes.")]
	private T[] _children = { };

	/// <summary>
	/// Gets a collection of objects associated with this group.
	/// </summary>
	public IReadOnlyList<T> objects => _children;

	/// <summary>
	/// Gets a readonly dictionary that maps a name to the object.
	/// </summary>
	public IReadOnlyDictionary<string, T> nameToObject
	{
		get
		{
			if (_nameToObject != null) return _nameToObject;

			_nameToObject = _children.ToDictionary(x => x.name, x => x);
			return _nameToObject;
		}
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		var duplicateNames = _children
			.GroupBy(x => x.name)
			.Where(g => g.Count() > 1)
			.ToList();

		if (duplicateNames.Count > 0)
		{
			StringBuilder builder = new();
			builder.AppendLine($"The {name} dialogue group has nodes with duplicate names. " +
				$"If not fixed, this will lead to an unexpected saving and loading " +
				$"behaviour.");
			builder.AppendLine();
			foreach (var group in duplicateNames)
			{
				builder.AppendLine($"\"{group.Key}\":");
				foreach (var node in group)
				{
					string assetPath = AssetDatabase.GetAssetPath(node);

					builder.AppendLine($"-- \"{assetPath}\"");
				}
				builder.AppendLine();
			}
			Debug.LogWarning(builder.ToString());
		}
	}

	/// <summary>
	/// Loads children objects from the base folder.
	/// </summary>
	public void LoadObjects()
	{
		string assetPath = AssetDatabase.GetAssetPath(this);
		string assetDirectory = Path.GetDirectoryName(assetPath);

		List<T> loadedObjects = new();
		var files = Directory.GetFiles(assetDirectory, "*.asset", SearchOption.AllDirectories);
		foreach (var file in files)
		{
			var node = AssetDatabase.LoadAssetAtPath<T>(file);
			if (node == null) continue;

			loadedObjects.Add(node);
		}
		var newChildren = loadedObjects.ToArray();
		// Check if the array was actually updated
		if (!ArrayUtility.ArrayEquals(_children, newChildren))
		{
			EditorUtility.SetDirty(this);
			_children = newChildren;
		}

		OnValidate();
	}
#endif
}
