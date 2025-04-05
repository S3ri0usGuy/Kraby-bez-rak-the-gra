using UnityEngine;

/// <summary>
/// A narration screen overlay.
/// </summary>
public class NarrationScreen : SingletonMonoBehaviour<NarrationScreen>
{
	private Animator _animator;

	private int _shownBoolId;

	[SerializeField]
	private string _shownBoolName = "shown";
	[SerializeField]
	private DialogueSpeaker _narratorSpeaker;

	/// <summary>
	/// Gets the dialogue speaker used for the narrator.
	/// </summary>
	public DialogueSpeaker narratorSpeaker => _narratorSpeaker;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_shownBoolId = Animator.StringToHash(_shownBoolName);
	}

	public void Show()
	{
		HUD.instance.Hide();
		_animator.SetBool(_shownBoolId, true);
	}

	public void Hide()
	{
		HUD.instance.Show();
		_animator.SetBool(_shownBoolId, false);
	}
}
