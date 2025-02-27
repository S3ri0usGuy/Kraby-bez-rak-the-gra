using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueAnswerParams))]
public class DialogueAnswerParamsDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		SerializedProperty answerTypeProp = property.FindPropertyRelative("_answerType");
		SerializedProperty timeToAnswerProp = property.FindPropertyRelative("_timeToAnswer");

		// "_answerType" field
		Rect answerTypeRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
		EditorGUI.PropertyField(answerTypeRect, answerTypeProp);

		// "_timeToAnswer" field
		EditorGUI.BeginDisabledGroup(answerTypeProp.enumValueIndex == (int)DialogueAnswerType.Default);

		Rect timeToAnswerRect = new(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
		EditorGUI.PropertyField(timeToAnswerRect, timeToAnswerProp);

		EditorGUI.EndDisabledGroup();

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUIUtility.singleLineHeight * 2 + 2;
	}
}
