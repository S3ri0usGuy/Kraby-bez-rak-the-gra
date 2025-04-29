using UnityEngine;

/// <summary>
/// A scriptable object which holds all dialogue nodes in the
/// same folder (including subfolders) it's located.
/// </summary>
[CreateAssetMenu(fileName = "Dialogue Group", menuName = "Dialogue/Group")]
public class DialogueGroup : ScriptableObjectGroup<DialogueNode> { }