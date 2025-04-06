using UnityEngine;

/// <summary>
/// A component that allows dialogues and other systems to show/hide the narration
/// screen.
/// </summary>
public class NarrationAction : MonoBehaviour
{
	public void ShowScreen()
	{
		NarrationScreen.instance.Show();
	}

	public void HideScreen()
	{
		NarrationScreen.instance.Hide();
	}
}
