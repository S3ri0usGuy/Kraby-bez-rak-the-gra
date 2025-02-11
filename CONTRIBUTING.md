# Guidelines and Suggestions

1. [Folder Structure Conventions](#folder-structure-conventions)  
   1.1 [3DModels](#3dmodels)  
   1.2 [Animations](#animations)  
   1.3 [Audio](#audio)  
   1.4 [Input](#input)  
   1.5 [Localization](#localization)  
   1.6 [Materials](#materials)  
   1.7 [Prefabs](#prefabs)  
   1.8 [Scenes](#scenes)  
   1.9 [Scripts](#scripts)  
   1.10 [Settings](#settings)  
   1.11 [Shaders](#shaders)  
   1.12 [Sprites](#sprites)  
   1.13 [Textures](#textures)  
2. [Pull Requests and Branches](#pull-requests-and-branches)  
3. [Commit Messages](#commit-messages)  
4. [Code Style Automation](#code-style-automation)  
5. [Warnings and Messages Policy](#warnings-and-messages-policy)  
6. [Access Modifier Guidelines](#access-modifier-guidelines)  
7. [Naming Conventions](#naming-conventions)  
8. [Naming Practices to Avoid](#naming-practices-to-avoid)  
   8.1 ["Manager" Suffix](#manager-suffix)  
   8.2 [Uninformative Variable Names](#uninformative-variable-names)  
   8.3 [Overuse of Acronyms](#overuse-of-acronyms)  
   8.4 [Singular Naming for Collections](#singular-naming-for-collections)  
   8.5 [General or Vague Names](#general-or-vague-names)  
9. [Comments](#comments)  
10. [Empty Lines](#empty-lines)  
11. [Logging and Error Handling](#logging-and-error-handling)

## Folder Structure Conventions
```
Assets/
├── 3DModels/
├── Animations/
├── Audio/
├── Input/
├── Localization/
├── Materials/
├── Prefabs/
├── Scenes/
├── Scripts/
├── Settings/
├── Shaders/
├── Sprites/
└── Textures/
```

### 3DModels

**Purpose**: Store all 3D model assets.

Each model should have its own dedicated folder to contain the model, its materials, 
textures, and animations (if they are specific to that model).

**Example:**
```
Assets/
  └── 3DModels/
      ├── Character/
      │   ├── CharacterModel.fbx
      │   ├── CharacterMaterial.mat
      │   ├── CharacterTexture.png
      │   └── CharacterAnimation.anim
      └── Environment/
          ├── TreeModel.fbx
          └── TreeTexture.png
```

### Animations
**Purpose**: Store animation clips and animators only.

This folder is dedicated to any animation data that isn’t tied directly to a 3D model (
which should remain in the 3DModels folder).

**Example:**
```
Assets/
  └── Animations/
      ├── NPC/
      │   └── NPC.controller
      │   └── NPC_Walk.anim
```

**A strong suggestion:** name all animations in a such way: `ModelName_AnimationAction` because it will significantly simplify finding them.

### Audio
**Purpose:** Store all audio files (e.g., sounds, music) and audio mixers.

**Example:**
```
Assets/
  └── Audio/
      ├── Music/
      │   └── MainTheme.wav
      ├── SFX/
      │   └── JumpSound.wav
      └── Mixers/
          └── MainAudioMixer.mixer
```

**Suggestion:** Follow this [website](https://www.gamedeveloper.com/audio/unity-audio-import-optimisation---getting-more-bam-for-your-ram).

### Input
**Purpose:** Store input configurations.

This folder is for storing the Unity Input System settings or any configuration data related to input mapping. Do not store scripts in this folder.
**Example:**

```
Assets/
  └── Input/
      └── PlayerInputActions.inputactions
```

### Localization
**Purpose:** Store all localization tables, string assets, and related data.

**Example:**
```
Assets/
  └── Localization/
      ├── English/
      │   └── Texts.asset
      └── Polish/
          └── Texts.asset
```

### Materials
**Purpose:** Store material assets for objects.

**Example:**
```
Assets/
  └── Materials/
      ├── PlayerMaterial.mat
      └── WallMaterial.mat
```

### Prefabs
**Purpose:** Store prefab assets, their variants, and related data in separate folders.

It's acceptable to place animations, textures, or materials in the prefab folder if they are specific to a single prefab group.

**Example:**
```
Assets/
  └── Prefabs/
      ├── Player/
      │   ├── PlayerPrefab.prefab
      │   ├── PlayerMaterial.mat
      │   └── PlayerAnimation.anim
      └── Environment/
          └── TreePrefab.prefab
```

### Scenes
**Purpose:** Store Unity scenes.

**Example:**
```
Assets/
  └── Scenes/
      ├── MainMenu.unity
      └── Level1.unity
```

### Scripts

**Purpose:** Store all C# scripts that are used in the game.

**Editor subfolder**: Editor folder contains scripts exclusive to the Unity editor (e.g., editor extensions, custom inspectors).

No scripts should be placed outside this folder, unless they are specific to test scenes. In that case, use a separate Assembly Definition for those.

**Example:**
```
Assets/
  └── Scripts/
      ├── Player/
      │   └── PlayerController.cs
      ├── Enemies/
      │   └── EnemyAI.cs
      └── Editor/
          └── CustomInspector.cs
```

### Settings
**Purpose:** Store render settings and settings specific to the render pipeline.

### Example:
```
Assets/
  └── Settings/
      └── URP-Balanced.asset
```

### Shaders
**Purpose:** Store ShaderLab shaders and shader graphs.

**Example:**
```
Assets/
  └── Shaders/
      ├── Blur.shader
      └── Water.shadergraph
```

### Sprites
**Purpose:** Store raster images that are used as Sprites, particularly for UI elements.

**Example:**
```
Assets/
  └── Sprites/
      ├── ButtonSprite.png
      └── BackgroundSprite.png
```

### Textures
**Purpose:** Store textures that are used for 3D models or as RawImage components on UI elements.

**Example:**
```
Assets/
  └── Textures/
      ├── GroundTexture.png
      └── SnowTexture.png
```

## Pull Requests and Branches

Try to avoid directly pushing to the `main`, unless your change is very small and you're 100% sure that it won't disrupt other team members. Such situations might include:

- `README` and other `.md` files changes.
- Very small fixes, for example, a typo in a script that only you're responsible for.

Instead of directly pushing to the `main`, create a new branch when working on a big feature, bug fix, or improvement. Use clear and descriptive branch names, such as:

- `player-movement`
- `fix-collision`
- `ui-tweaks`

Before merging your branch into the main (or develop) branch, open a pull request. Even when not using code reviews, a pull request might be an excellent way to document big changes and track the project's history.

- Keep PR descriptions clear and concise.
- Link related issues if applicable.
- Code reviews are optional but encouraged for major changes.

## Commit Messages

When making commits to this repository, please adhere to the following guidelines for writing commit messages:

- Start the message with a verb in the imperative mood, with the first letter capitalized.
- Use present tense for describing what the commit does.
- Keep the message concise and descriptive.

Example:
- ❌ "Enhanced player movement"
- ✅ "Enhance player movement"
- ✅ "Fix collision detection issue"

## Code Style Automation

When writing code make sure that your IDE supports the `.editorconfig` file (Visual Studio and Rider support it by default).

### Visual Studio 2022 Tips
To apply the formatting manually use the `Ctrl K + Ctrl D` shortcut.

Additionaly you can enable the `Run Code Cleanup profile on Save` setting to automatize this action (strongly advised, but it can sometimes be intrusive):

1. Go to `Tools -> Options`.
2. Find the `Text Editor` section.
3. Select the `Code Cleanup` subsection.
4. Enable `Run Code Cleanup profile on Save`.

## Warnings and Messages Policy
- Keep the project free of all compile-time warnings and messages thrown by Visual Studio or Unity.
- If a warning appears, fix it instead of ignoring it.
- If you believe a warning is incorrect or unnecessary, suppress it explicitly and provide a comment explaining why.
- Maintaining 0 warnings and messages ensures that any new and important warnings stand out and get noticed.

**Note:** these policies are for **compile-time** warnings and messages only. For the `Debug.Log` message conventions read [this section](#logging-and-error-handling).

## Access Modifier Guidelines

- Use the `private` access modifier by default unless there's a specific reason to use a different modifier.
- Try to avoid fields that are not defined as `private` or `private static`.
  - Use public/protected properties or methods to expose necessary fields. 
  This might seem redundant, but it helps enforce encapsulation, maintain control over data, and prevent unintended modifications. 
  The setters and getters are better than default fields because they can be changed to include an additional logic, such as validation
  or mapping to a different field.
  
  ```csharp
  // Wrong ❌ - violates encapsulation, every class can modifiy this field which can lead to an unexpected behaviour.
  public float foo;

  // Correct ✅ - when only read access is needed.
  private float _foo;
  public float foo => _foo;

  // Correct ✅ - when write access is needed.
  private float _foo;
  public float foo
  {
     get => _foo;
     set => _foo = value;
  }
  ```
  - Use `SerializeFieldAttribute` when you need to make a field modificable in the editor:
  ```csharp
  // Wrong ❌ - violates encapsulation, every class can modifiy this field which can lead to an unexpected behaviour.
  public float myUnityField = 0f;

  // Correct ✅
  [SerializeField]
  private float _myUnityField = 0f;
  ```

## Naming Conventions

To maintain consistency across the codebase, please adhere to the following naming conventions:

- **Classes, Methods, Delegates and Enums:** PascalCase:
```csharp
public class MyClass
{
  private void MyMethod1()
  {
    // ...
  }

  public void MyMethod2()
  {
    // ...
  }
}
```
- **Interfaces:** PascalCase with a leading 'I':
```csharp
public interface IDamagable
{
  void Damage(int damage);
}
```
- **Fields:**
  - camelCase with a leading underscore for all kinds of private fields:
  ```csharp
  private float _myField;

  private static float _myStaticField;
  
  [SerializeField]
  private float _myUnityField;
  ```
  
  - camelCase for fields with a different access modifier (e.g., `public`, `protected`):
  ```csharp
  protected int foo;

  public int bar; // Try to avoid public fields as they may violate the encapsulation
  ```
- **Local Variables and Arguments:** camelCase:
```csharp
private void Method(int someArgument) 
{ 
    int localVariable = 69;
}
```
- **Properties:** camelCase:
```csharp
protected void property1 { get; private set; }
public void property2 { get; private set; }
public void property3 { get; set; }
```
- **Events:** camelCase with a past tense verb:
```csharp
// C# built-in event:
public event Action enemyKilled;

// Unity events:
[SerializeField]
private UnityEvent _enemyKilled;

public UnityEvent enemyKilled => _enemyKilled;
```

## Naming Practices to Avoid

### "Manager" Suffix

When naming classes in your codebase, avoid using the "Manager" suffix unless it accurately reflects the class's responsibilities as a designated manager.

```csharp
// Wrong ❌:
public class RoomManager : MonoBehaviour
{
  // ...
}

// Correct ✅:
public class Room : MonoBehaviour
{
  // ...
}
```

### Uninformative Variable Names

Avoid using generic names like `temp`, `data`, or single-letter variables (except in loops or lambdas) that do not provide sufficient information about their purpose:

```csharp
// Bad (outside of a loop) ❌
int i; // What does 'i' represent?

// Bad ❌
Vector3 temp; // What is 'temp' used for?

// Better ✅
int attackIndex; // Clear and descriptive

// Better ✅
Vector3 targetDirection; // Indicates what information this variable holds
```

### Overuse of Acronyms

When naming variables, methods, classes, or other elements in your codebase, avoid using acronyms unless they are widely understood 
(e.g. hp) or necessary due to length constraints.

```csharp
// Bad ❌: Unclear abbreviation
int atcIdx; // What does 'atc' stand for?

// Acceptable 👌: 'rb' is commonly used for Rigidbody in Unity, but may still cause confusion
Rigidbody rbPlayer;

// Better ✅: Explicit and understandable
int attackIndex;

// Better ✅: Descriptive and clear
Rigidbody playerRigidbody;

// Correct ✅: Widely understood acronym
float hp;
```

### Singular Naming for Collections

Avoid using singular nouns when naming collection variables (lists, arrays, hash sets, etc.).
Collections inherently represent multiple items, so their names should reflect this:

```csharp
// Wrong ❌: Singular naming for arrays
Vector3[] point;

// Correct ✅: Plural naming for arrays
Vector3[] points;

// Wrong ❌: Singular naming for collections
List<GameObject> entry;

// Correct ✅: Plural naming for collections
List<GameObject> entries;
```

**Exception:** it's acceptable to use a singular naming for collections that represent a logical single entity:
```csharp
Dictionary<int, GameObject> objectById; // The dictionary maps a single ID to a single object
```

### General or Vague Names
To maintain clarity and avoid ambiguity, do not use overly general or vague script names. Each script should clearly describe its purpose or the system it interacts with.

```csharp
// Wrong ❌:
public class Manager; // Too vague
public class DataHandler;  // What data? What kind of handling?

// Correct ✅:
public class PlayerController;  // Specific to player movement and input
public class EnemyAI;  // Manages enemy behavior and decision-making
```

## Comments

When writing comments in the codebase, adhere to the following guidelines:

* Use a space after the // for single-line comments:
```csharp

// Wrong ❌:
int four = 2 + 2; //This is an addition

// Correct ✅:
int four = 2 + 2; // This is an addition
```

* Use [XML comments](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags) for `public`/`protected` members. Avoid using them for `private` members. Finish each sentence with a period:
```csharp
using System;
using UnityEngine;

/// <summary>
/// Interface that represents a status effect.
/// </summary>
public interface IStatusEffect
{
  /// <summary>
  /// Gets the count of status effects of this type applied at once. 
  /// Should be 1 by default and for non stackable effects.
  /// </summary>
  public int count { get; }
  
  /// <summary>
  /// An event that is called when status effect is updated.
  /// </summary>
  public event Action<IStatusEffect, StatusEffectConfig> updated;
  
  /// <summary>
  /// An event that is called when status effect is deactivated.
  /// </summary>
  public event Action<IStatusEffect, StatusEffectConfig> deactivated;
  
  /// <summary>
  /// Initializes a status effect.
  /// </summary>
  /// <param name="receiver">The object to which the status effect is applied.</param>
  /// <param name="applier">Applier of a status effect.</param>
  public void Init(StatusEffectsReceiver receiver, Component applier);
  
  // ...
}
```

* Use `TooltipAttribute` for Unity fields. Tooltips appear directly in the Unity Editor's Inspector when you hover over the parameter:
```csharp
// Ok 👌:
// Critical damage chance in range [0, 1].
[SerializeField]
private float m_critChance = 1f;

// Better ✅:
[SerializeField, Range(0f, 1f)]
[Tooltip("Critical damage chance in range [0; 1].")]
private float m_critChance = 1f;
```

* Try to avoid commented-out code in the repository.
   * If code is no longer needed, delete it instead of commenting it out.
   * If you need to keep an old version of code, rely on version control rather than leaving it in the file.

## Empty Lines

1. Use a single blank line to separate logical blocks of code. Logical blocks can include class definitions, method definitions, and different sections within a method.
2. Do not leave two or more blank lines in a row. Consecutive blank lines can make the code harder to read and follow, and they do not provide any additional clarity.
3. Use a single blank line to separate methods within a class. A single blank line can also be used to separate some fields or properties.

```csharp
// Wrong ❌:
public class ExampleClass1
{
  public void Method1()
  {
    // Some code here



    // Some additional code here

  }


  public void Method2()
  {
    // Some code here
  }

}

// Correct ✅:
public class ExampleClass2
{
  public void Method1()
  {
    // Some code here

    // Some additional code here
  }

  public void Method2()
  {
    // Some code here
  }
}
```

## Logging and Error Handling

- Avoid unnecessary error handling when built-in Unity mechanisms can enforce constraints.
Examples: 
    - Instead of manually checking if a required component exists, use `[RequireComponent(typeof(ComponentType))]` to ensure it’s attached.
    - Instead of checking if some parameter is in a specific range use `[Range(minValue, maxValue)]` or `[Min(minValue)]
- Remove unnecessary `Debug.Log` statements after debugging to avoid cluttering logs and disrupting teammates.
    - Use logging intentionally - only log meaningful events, warnings, or errors.
    - Use `Debug.LogWarning` for potential issues and `Debug.LogError` for critical errors.
    - Consider wrapping logs in `#if UNITY_EDITOR` if they are only useful for debugging during development.