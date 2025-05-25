using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <Summary>
/// Controls Main Menu: Changes scene and quits game
/// </Summary>
/// TO DO: Integrate save system better
public class MainMenuManager : MonoBehaviour // should be singleton ? - nah, doesn't matter
{
	private SaveSlot _loadedSlot;

	[SerializeField] private string _continueGame;
	[SerializeField] private Button _continueButton;
	[SerializeField] private string _defaultSlotName = "default";

	private void Start()
	{
		_loadedSlot = new(_defaultSlotName);
		ISavePersistence persistence = SavePersistenceFactory.CreatePersistence();

		bool isLoaded = persistence.Load(_loadedSlot);
		_continueButton.gameObject.SetActive(isLoaded);
	}

	private void StartGame()
	{
		SceneManager.LoadSceneAsync(_continueGame);
	}

	public void NewGame()
	{
		SaveSlot newSlot = new(_defaultSlotName);
		SaveSystem.SetSave(newSlot);
		StartGame();
	}
	public void ContineGame()
	{
		SaveSystem.SetSave(_loadedSlot);
		StartGame();
	}
	public void QuitGame()
	{
		Application.Quit();
	}
}