using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EndingScreen : SingletonMonoBehaviour<EndingScreen>
{
	private Animator _animator;

	[SerializeField]
	private string _showTriggerName = "show";

	private void Start()
	{
		_animator = GetComponent<Animator>();
	}

	public void Show()
	{
		_animator.SetTrigger(_showTriggerName);
	}

	public void Quit()
	{
		Debug.Log("Quit game.");
		Application.Quit();
	}
}
