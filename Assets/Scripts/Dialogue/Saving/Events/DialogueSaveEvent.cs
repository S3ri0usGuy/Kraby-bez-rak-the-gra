using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/Save Event")]
public class DialogueSaveEvent : ScriptableObject
{
	[SerializeField]
	[Tooltip("A unique identifier of the event.")]
	private string _id = "dialogue_event";

	/// <summary>
	/// Gets the unique identifier of the event
	/// </summary>
	public string id => _id;
}
