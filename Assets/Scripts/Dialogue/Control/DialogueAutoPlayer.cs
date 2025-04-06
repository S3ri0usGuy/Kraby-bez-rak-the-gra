using UnityEngine;

/// <summary>
/// A component that auto plays the dialogue until it ends on the same node twice.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(DialoguePlayer))]
public class DialogueAutoPlayer : MonoBehaviour
{
	private DialoguePlayer _player;

	private void Awake()
	{
		_player = GetComponent<DialoguePlayer>();
	}

	private void OnEnable()
	{
		_player.ended.AddListener(OnDialogueEnded);
	}

	private void OnDialogueEnded()
	{

	}
}
