using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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

	[SerializeField, Min(0)]
	[Tooltip("An index of the phrase which activates the trigger.")]
	private int _phraseIndex = 0;

	[SerializeField]
	[Tooltip("An event that is called when the phrase starts.")]
	[FormerlySerializedAs("_started")]
	private UnityEvent _phraseStarted;

	[SerializeField]
	[Tooltip("An event that is called when the node ends.")]
	private UnityEvent _ended;

	/// <summary>
	/// Gets an index of the phrase that activates the <see cref="phraseStarted" /> event.
	/// </summary>
	public int phraseIndex => _phraseIndex;

	/// <summary>
	/// An event that is called when the specified phrase starts.
	/// </summary>
	public UnityEvent phraseStarted => _phraseStarted;

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
		if (!IsTargetNode(context.node)) return;

		if (_phraseIndex >= context.node.phrases.Count)
		{
			Debug.LogWarning($"The phraseIndex ({_phraseIndex}) is incorrect and the " +
				$"dialogue node trigger ({context.node.name}) will never start.",
				gameObject);
			return;
		}

		if (context.phraseIndex == _phraseIndex)
		{
			_phraseStarted.Invoke();
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
