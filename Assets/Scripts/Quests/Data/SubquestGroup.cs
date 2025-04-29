using UnityEngine;

/// <summary>
/// A scriptable object which holds all subquests in the
/// same folder (including subfolders) it's located.
/// </summary>
[CreateAssetMenu(fileName = "SubquestGroup", menuName = "Quests/Groups/Subquest Group", order = 1000000)]
public sealed class SubquestGroup : ScriptableObjectGroup<Subquest> { }
