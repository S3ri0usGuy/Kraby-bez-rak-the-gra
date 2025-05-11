# Save System

The save system consists of 3 layers:

- **Persistence:** the `ISavePersistence` interface defines a persistence class which is used to save, load and delete save slots. The `SavePersistenceFactory` class provides an appropriate save persistence implementation for the current platform.
- **System:** the `SaveSystem` singleton provides the saving functionality. It has its own prefab (`Prefabs/SaveSystem`) that must be placed on all scenes that have something worth saving. This singleton keeps track of all savable objects and calls the save functionality when needed. The savable objects have an access to their data right after they are registered.
- **Savables:** objects that implement the `ISavable` interface. The most notable one is the `SavableComponent<TData>` which can be used to save anything on the scene. An example of how these components are used can be seen in the `PositionSaver` class. These objects are supposed to register themselves using the `SaveSystem` singleton.

### Save Slots
A slot refers to a uniquely named container used to organize and manage save data. Each slot represents an independent game state, allowing multiple playthroughs or save points for a single user. Slots are identified by a name that contains only alphanumerical characters, including '-' and '_' (`^[a-zA-Z0-9_-]*$`).

Slots are managed by the persistence layer.

## Savable Components

Savable components are components that inherit the `SavableComponent<TData>` class. They usually have the `Saver` suffix.

Each savable component must have its unique **"Id"**. It can be set in the editor. Please, use this format: `my-component-id` (lowercase letters, words are separated by `-`) when naming IDs.

### Savers Key Features

- **Generic Data Type**: Each component defines its own data structure (`TData`) that must be a serializable reference type (use the `[System.Serializable]` attribute).
- **Automatic Registration**: Components register and unregister themselves with the `SaveSystem` in `Awake` and `OnDestroy`, respectively.
- **Auto Save Support**: Changes can request auto-saving via the `RequestAutoSave()` method, provided the system is not currently loading.
- **Fallback Data**: When loading fails, fallback data can be provided by overriding the `fallbackData` property.
- **Validation**: You can validate loaded and saved data by overriding the `Validate(TData)` method.
- **Custom Hooks**: Override `OnLoad()` and `OnSave()` to inject behavior after loading or before saving data.

### The Most Important Savers

- `PositionSaver` - saves the position and rotation of the object.

## Saves Format

By default, the Save System uses JSON format to save data. Files are not encrypted so players can potentially manipulate them. 

On windows, the saves are located in the `C:\Users\[User]\AppData\LocalLow\Kraby bez Rąk\The Last Day` folder (you can open it using `Saves -> Open saves folder`).

## Autosaving

The game has an autosaving feature. If it's enabled the game will automatically be saved when quest or dialogue states change.

## Editor

If the save system was not initialized it will use the "dev save slots" by default. Such slots have a name in format: `dev-[SceneName]`.

### Save Tools
There are save tools that can be accessed in the Unity menu.
 
![image](https://github.com/user-attachments/assets/2c68cf58-05d4-440a-87a6-4293185afe97)

The `Save` button works only when the play mode is active and there is a `SaveSystem` component on a current scene.

## Known Limitations

- The system currently cannot see available save slots.
- The error handling might not be production ready.

## Expected Flow
1. The main menu loads a few slots (from 3 to 10) (using the persistence layer).
2. The player has an option to choose any slot (even if it doesn't yet exist) and has an option to delete already existing ones.
3. After the player has chosen a slot, the main menu sets it as a current.
4. Then a new scene is loaded. This scene has an instance of the `SkillSystem` prefab and savable components (that inherit the `SavableComponent<TData>` class).
5. The data is sent to the components in the `Awake`.
6. The player plays the game normally.
7. The save function is called. It can be called either manually or automatically, the current system doesn't care.
8. The savable components provide an updated data.
9. The save system calls the persistence layer to save the updated data.

## Usage

### Saving data (user input)
```csharp
SaveSystem.instance.Save();
```

### Saving data (autosave) 

If your class inherits the `SavableComponent<TData>` then use the `RequestAutoSave` method (the safest approach):

```csharp
public class MySaver : SavableComponent<MySaverData>
{
    // ...

	private void OnSomethingChanged() 
    {
		RequestAutoSave();
	}

    // ...
```

Otherwise, you can call the auto saving directly. Just make sure that you don't do this during the loading:

```csharp
SaveSystem.instance.RequestAutoSave();
```

Note that both methods do not work if the auto saving is disabled (`SaveSystem.autoSaveEnabled == false`).

### Enabling/Disabling Autosaving

```csharp
// Enables autosaving
SaveSystem.autoSaveEnabled = true;

// Disables autosaving
SaveSystem.autoSaveEnabled = false;
```

### Loading slots
```csharp
SaveSlot slot = new SaveSlot(slotName);
ISavePersistence persistence = SavePersistenceFactory.CreatePersistence();

if (persistence.Load(slot))
{
    // Slot was successfully loaded (data inside it still may be not valid)
    // ...
}
else
{
    // Slot doesn't exist, a new game is started
    // ... 
}
```
Remember to handle the `CorruptedSaveException`, otherwise funny stuff can happen.

### Switching/Setting slots
```csharp
// Assuming that 'slot' was loaded
SaveSystem.SetSave(slot);
```

### Deleting slots
```csharp
ISavePersistence persistence = SavePersistenceFactory.CreatePersistence();
persistence.Delete(slotName);
```

### Saved Event

You can use the `SaveSystem.saved` event to add the saving callback (e.g. showing the save icon).

```csharp
SaveSystem.instance.saved += OnSaveComplete;

private void OnSaveComplete()
{
    Debug.Log("Game saved successfully.");
}
```

Don't forget to unsubscribe from the event if your component lives longer then the `SaveSystem` for some reason (0.01% chance of happening).