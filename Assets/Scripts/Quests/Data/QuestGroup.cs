using UnityEngine;

/// <summary>
/// A scriptable object which holds all quests in the
/// same folder (including subfolders) it's located.
/// </summary>
[CreateAssetMenu(fileName = "QuestGroup", menuName = "Quests/Groups/Quest Group", order = 999999)]
public sealed class QuestGroup : ScriptableObjectGroup<Quest> { }
