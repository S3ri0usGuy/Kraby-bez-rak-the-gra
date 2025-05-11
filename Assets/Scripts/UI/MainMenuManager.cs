using UnityEngine;
using UnityEngine.SceneManagement;

//<Summary>
//Controls Main Menu: Changes scene and quits game
//</Summary>
//TO DO: Integrate save system, refactor change scene function

public class MainMenuManager : MonoBehaviour //should be singleton ?
{
	[SerializeField] private string _continueGame;
	public void ContineGame()
	{
		SceneManager.LoadSceneAsync(_continueGame);
	}
	public void QuitGame()
	{
		Application.Quit();
	}
}