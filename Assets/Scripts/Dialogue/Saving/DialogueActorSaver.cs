using UnityEngine;

/// <summary>
/// A component that allows to save the <see cref="DialogueActor" /> state.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(DialogueActor))]
public class DialogueActorSaver : SavableComponent<DialogueSaveData>
{
	private DialogueGroup _sharedGroup;

	private DialogueActor _actor;

	[SerializeField]
	[Tooltip("A group of dialogue nodes that the actor uses.")]
	private DialogueGroup _group;

	protected override DialogueSaveData fallbackData => new();

	protected override void Awake()
	{
		_actor = GetComponent<DialogueActor>();
		_actor.nextNodeChanged += OnNextNodeChanged;

		_sharedGroup = SharedDialogueGroupProvider.exists ?
			SharedDialogueGroupProvider.instance.sharedGroup : null;

		base.Awake();
	}

	private void OnNextNodeChanged(DialogueNode node)
	{
		RequestAutoSave();
	}

	protected override void Validate(DialogueSaveData data)
	{
		if (string.IsNullOrEmpty(data.nextNodeName) || !_group) return;

		if (!_group.nameToObject.ContainsKey(data.nextNodeName) &&
			(!_sharedGroup || !_sharedGroup.nameToObject.ContainsKey(data.nextNodeName))
			)
		{
			Debug.LogWarning($"Couldn't find a dialogue node with \"{data.nextNodeName}\" name.", gameObject);
			data.nextNodeName = null;
		}
	}

	protected override void OnLoad()
	{
		if (!_group)
		{
			Debug.LogError($"The \"{gameObject.name}\" has no dialogue group assigned to it.", gameObject);
			return;
		}

		if (!string.IsNullOrEmpty(data.nextNodeName) &&
			(_group.nameToObject.TryGetValue(data.nextNodeName, out var node) ||
			_sharedGroup && _sharedGroup.nameToObject.TryGetValue(data.nextNodeName, out node))
			)
		{
			_actor.SetNextNode(node);
		}
	}

	protected override void OnSave()
	{
		data.nextNodeName = _actor.nextNode.name;
	}
}
