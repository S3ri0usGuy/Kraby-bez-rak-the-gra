using UnityEngine;

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

	private void Awake()
	{
		_system = GetComponent<SaveSystem>();
		if (_system.currentSaveSlot == null)
		{
			SaveSlot devSave = new(_developmentSlotName);

			var persistence = SavePersistenceFactory.CreatePersistence();
			persistence.Load(devSave);

			SaveSystem.SetSave(devSave);

			// If this appears in production then something is wrong
			Debug.Log("The development save slot is used.");
		}
	}
}
