using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueGroup))]
public class DialogueGroupEditor : Editor
{
	public override void OnInspectorGUI()
	{
		// Draw the default inspector fields
		DrawDefaultInspector();

		// Add a button
		DialogueGroup group = (DialogueGroup)target;
		if (GUILayout.Button("Update Groups"))
		{
			// Call the method when the button is clicked
			group.LoadNodes();
		}
	}
}
