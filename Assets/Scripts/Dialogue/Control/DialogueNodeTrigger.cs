using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that triggers events when the node starts or ends.
/// </summary>
/// <remarks>
/// This component must be a child of the <see cref="DialogueActor"/> component.
/// </remarks>
public class DialogueNodeTrigger : MonoBehaviour
{
	private DialogueActor _actor;

	[SerializeField]
	[Tooltip("A node that triggers this component. If empty, all nodes trigger this component.")]
	private DialogueNode _targetNode;

	[SerializeField]
	[Tooltip("An event that is called when the node starts.")]
	private UnityEvent _started;

	[SerializeField]
	[Tooltip("An event that is called when the node ends.")]
	private UnityEvent _ended;

	/// <summary>
	/// An event that is called when the node ends.
	/// </summary>
	public UnityEvent started => _started;

	/// <summary>
	/// An event that is called when the node ends.
	/// </summary>
	public UnityEvent ended => _ended;

	private void Awake()
	{
		_actor = this.TryGetInParent<DialogueActor>();

	}

	private void OnEnable()
	{
		_actor.phraseStarted += OnPhraseStarted;
		_actor.nodeEnded += OnNodeEnded;
	}

	private void OnDisable()
	{
		_actor.nodeEnded -= OnNodeEnded;
	}

	private bool IsTargetNode(DialogueNode node)
	{
		return !_targetNode || _targetNode == node;
	}

	private void OnPhraseStarted(DialoguePhraseContext context)
	{
		if (context.phraseIndex == 0 && IsTargetNode(context.node))
		{
			_started.Invoke();
		}
	}

	private void OnNodeEnded(DialogueNode node, DialogueNode nextNode)
	{
		if (IsTargetNode(node))
		{
			_ended.Invoke();
		}
	}
}
