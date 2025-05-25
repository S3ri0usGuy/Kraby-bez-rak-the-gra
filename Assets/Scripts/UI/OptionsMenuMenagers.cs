using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//<summary>
//this currently menageges audio setting with each volume option having its own function.
//In future this will also manage other settings
//using PlayerPrefs it saves used settings
//</summary>
public class OptionsMenuMenagers : MonoBehaviour
{
	[SerializeField] private AudioMixer _mainAudioMixer;
	[SerializeField] private Slider _masterVolumeSlider;
	[SerializeField] private Slider _sfxVolumeSlider;
	[SerializeField] private Slider _dialogueVolumeSlider;
	

	private void Awake()//loads settings
	{
		if (PlayerPrefs.HasKey("MasterVolume"))
		{
			LoadSettings();
		}		
		SetMasterVolume();
		SetDialogueVolume();
		SetSFXVolume();
	}

	public void SetMasterVolume()
	{
		float volume = _masterVolumeSlider.value;
		_mainAudioMixer.SetFloat("Master", volume);
		PlayerPrefs.SetFloat("MasterVolume", volume);
	}
	public void SetDialogueVolume()
	{
		float volume = _dialogueVolumeSlider.value;
		_mainAudioMixer.SetFloat("Dialogue", volume);
		PlayerPrefs.SetFloat("DialogueVolume", volume);
	}

	public void SetSFXVolume()
	{
		float volume = _sfxVolumeSlider.value;
		_mainAudioMixer.SetFloat("SFX", volume);
		PlayerPrefs.SetFloat("SFXVolume", volume);
	}
	
	//later this will load all of the settings
	private void LoadSettings()
	{
		_masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
		_dialogueVolumeSlider.value = PlayerPrefs.GetFloat("DialogueVolume");
		_sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
	}
}
