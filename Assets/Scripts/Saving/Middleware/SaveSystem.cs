using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// A Unity object singleton which tracks savable objects,
/// keeps the save slot in memory and allows data saving.
/// </summary>
public sealed class SaveSystem : SingletonMonoBehaviour<SaveSystem>
{
	private static SaveSlot _slot;

	private readonly HashSet<ISavable> _savables = new();

	/// <summary>
	/// Gets a current save slot. Nullable.
	/// </summary>
	/// <remarks>
	/// Be warned that changing this object's values directly might lead 
	/// to an unexpected behaviour.
	/// </remarks>
	public SaveSlot currentSaveSlot => _slot;

	/// <summary>
	/// Starts tracking a savable object and initializes it with a save data.
	/// </summary>
	/// <remarks>
	/// Don't forget to call the <see cref="RemoveSavable(ISavable)" />
	/// when the savable is no longer needed.
	/// </remarks>
	/// <param name="savable">A savable object to initialize and start tracking.</param>
	/// <exception cref="InvalidOperationException">
	/// Thrown when an already added savable was attempted to be added
	/// again.
	/// </exception>
	public void AddSavable(ISavable savable)
	{
		if (savable == null)
			throw new ArgumentNullException(nameof(savable));

		if (!_savables.Add(savable))
			throw new InvalidOperationException("Attempted to add the same savable twice.");

		var data = _slot.saveData.componenentsData.GetValueOrDefault(savable.id);
		savable.Load(data);
	}

	/// <summary>
	/// Stops tracking the savable object.
	/// </summary>
	/// <remarks>
	/// Has no effect if the savable wasn't tracked.
	/// </remarks>
	/// <param name="savable">A savable object to stop tracking.</param>
	public void RemoveSavable(ISavable savable)
	{
		if (savable == null)
		{
			throw new ArgumentNullException(nameof(savable));
		}

		_savables.Remove(savable);
	}

	/// <summary>
	/// Sets the save slot used globally.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This method works even if there is no active <see cref="SaveSystem" /> on the scene and
	/// works between scenes.
	/// </para>
	/// <para>
	/// <b>Note:</b> the passed save slot must be is not loaded in this method.
	/// In order to load the save slot use the <see cref="SavePersistenceFactory.CreatePersistence" /> 
	/// and <see cref="ISavePersistence.Load(SaveSlot)" /> methods.
	/// </para>
	/// </remarks>
	/// <param name="slot">The save slot to be set as the current.</param>
	public static void SetSave(SaveSlot slot)
	{
		if (slot.saveData == null)
			slot.saveData = new();
		if (slot.saveData.componenentsData == null)
			slot.saveData.componenentsData = new();

		_slot = slot ?? throw new ArgumentNullException(nameof(slot));
	}

	/// <summary>
	/// Saves registered savable objects to the current slot.
	/// </summary>
	public void Save()
	{
		if (_slot == null)
		{
			throw new InvalidOperationException("Attempted to save but there is no current save slot set.");
		}

#if UNITY_EDITOR
		// Find non-unique IDs
		Dictionary<string, List<ISavable>> idSavableList = new();
		foreach (var savable in _savables)
		{
			if (idSavableList.ContainsKey(savable.id))
			{
				idSavableList[savable.id].Add(savable);
			}
			else
			{
				idSavableList[savable.id] = new() { savable };
			}
		}

		foreach (var (id, savables) in idSavableList)
		{
			if (savables.Count <= 1) continue;

			StringBuilder builder = new();
			Debug.LogWarning($"Duplicated ({savables.Count}) save IDs were found: '{id}'.");

			foreach (var savable in savables)
			{
				if (savable is UnityEngine.Object unityObject)
				{
					Debug.LogWarning($"The '{unityObject.name}' has a non-unique save ID.", unityObject);
				}
			}
		}
#endif

		foreach (var savable in _savables)
		{
			_slot.saveData.componenentsData[savable.id] = savable.Save();
		}

		ISavePersistence provider = SavePersistenceFactory.CreatePersistence();
		provider.Save(_slot);
	}
}

/// <summary>
/// An action for the event which is called right before the player's data is saved.
/// </summary>
/// <param name="save">A save object which is the same as the <see cref="SaveSystem.save" />.</param>
public delegate void SavingAction(SaveSlot save);
