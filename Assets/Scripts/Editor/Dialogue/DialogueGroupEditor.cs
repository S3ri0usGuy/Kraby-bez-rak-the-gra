using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueGroup))]
public class DialogueGroupEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		DialogueGroup group = (DialogueGroup)target;
		if (GUILayout.Button("Update Nodes"))
		{
			group.LoadNodes();
		}
	}
}
