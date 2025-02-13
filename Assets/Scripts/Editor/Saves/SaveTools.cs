using UnityEditor;
using UnityEngine;

public static class SaveTools
{
	[MenuItem("Saves/Open saves folder", false, 1)]
	public static void OpenSavesFolder()
	{
		EditorUtility.RevealInFinder(Application.persistentDataPath + "/");
	}

	[MenuItem("Saves/Save", false, 100)]
	public static void Save()
	{
		SaveSystem.instance.Save();
	}

	[MenuItem("Saves/Open current save folder", true)]
	[MenuItem("Saves/Save", true)]
	public static bool SaveSystemExists()
	{
		return Application.isPlaying && SaveSystem.exists;
	}
}
