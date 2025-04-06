using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// A component that controls the player state and view during the dialogue.
/// </summary>
[RequireComponent(typeof(DialoguePlayer))]
public class DialoguePlayerLocker : MonoBehaviour
{
	private DialoguePlayer _dialoguePlayer;

	[SerializeField]
	[Tooltip("An optional camera that is used during the dialogue.")]
	private CinemachineCamera _camera;

	[SerializeField]
	[Tooltip("An optional target where the player looks at.")]
	private Transform _lookTarget;

	private void Awake()
	{
		_dialoguePlayer = GetComponent<DialoguePlayer>();
		_dialoguePlayer.started.AddListener(OnDialogueStarted);
		_dialoguePlayer.ended.AddListener(OnDialogueEnded);
	}

	private void OnDialogueStarted()
	{
		if (_camera)
		{
			_camera.enabled = true;
		}
		PlayerLocker.Lock();
		if (Player.exists && _lookTarget)
		{
			var player = Player.instance;
			player.rotation.ForceRotation(_lookTarget.position - player.transform.position);
		}
	}

	private void OnDialogueEnded()
	{
		if (_camera)
		{
			_camera.enabled = false;
		}
		PlayerLocker.Unlock();
	}
}
