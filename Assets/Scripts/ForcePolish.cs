using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ForcePolishLanguage : MonoBehaviour
{
	private IEnumerator Start()
	{
		// Wait for the localization system to initialize
		yield return LocalizationSettings.InitializationOperation;

		// Find the Polish locale
		Locale polishLocale = null;
		foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
		{
			if (locale.Identifier.Code == "pl-PL")
			{
				polishLocale = locale;
				break;
			}
		}

		if (polishLocale != null)
		{
			LocalizationSettings.SelectedLocale = polishLocale;
		}
		else
		{
			Debug.LogWarning("Polish locale not found.");
		}
	}
}
