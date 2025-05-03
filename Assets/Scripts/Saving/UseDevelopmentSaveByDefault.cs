using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A component that enforces the development save slot if 
/// a slot was not previously set.
/// </summary>
[RequireComponent(typeof(SaveSystem))]
public class UseDevelopmentSaveByDefault : MonoBehaviour
{
	private SaveSystem _system;

	[SerializeField]
	private string _developmentSlotName = "dev";
	[SerializeField]
	private bool _useSceneName = true;

	private void Awake()
	{
		_system = GetComponent<SaveSystem>();
		if (_system.currentSaveSlot == null)
		{
			string slotName = _developmentSlotName;
			if (_useSceneName)
			{
				string sceneName = SceneManager.GetActiveScene().name
					.Replace(" ", "")
					.Trim();
				slotName = $"{slotName}-{sceneName}";
			}
			SaveSlot devSave = new(slotName);

			var persistence = SavePersistenceFactory.CreatePersistence();
			persistence.Load(devSave);

			SaveSystem.SetSave(devSave);

			// If this appears in production then something is wrong
			Debug.Log($"The development save slot is used (\"{slotName}\").");
		}
	}
}
