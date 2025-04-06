using UnityEngine;

public class EndGameAction : MonoBehaviour
{
	public void EndGame()
	{
		EndingScreen.instance.Show();
	}
}
