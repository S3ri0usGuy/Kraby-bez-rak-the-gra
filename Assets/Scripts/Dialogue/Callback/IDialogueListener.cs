/// <summary>
/// An interface for the object that listens to the <see cref="DialoguePlayer" />
/// callbacks.
/// </summary>
public interface IDialogueListener
{
	/// <summary>
	/// Called when the phrase starts playing.
	/// </summary>
	/// <param name="context">The context of the phrase that has started playing.</param>
	public void OnPhraseStarted(DialoguePhraseContext context);

	/// <summary>
	/// Called when the phrase ends playing.
	/// </summary>
	/// <param name="context">The context of the phrase that has ended playing.</param>
	public void OnPhraseEnded(DialoguePhraseContext context);

	/// <summary>
	/// Called when a one of the player options is chosen.
	/// </summary>
	/// <remarks>
	/// This method is called before the <see cref="OnNodeEnded(DialogueNode, DialogueNode)" />
	/// and is skipped when the phrase has no options.
	/// </remarks>
	/// <param name="context">The context of the option that has been chosen.</param>
	public void OnOptionChosen(DialogueOptionContext context);

	/// <summary>
	/// Called when the dialogue node has ended playing.
	/// </summary>
	/// <param name="node">The dialogue node that has ended playing.</param>
	/// <param name="nextNode">
	/// The next node that will be played. Can be <see langword="null" /> if
	/// the dialogue has ended completely.
	/// </param>
	public void OnNodeEnded(DialogueNode node, DialogueNode nextNode);

	/// <summary>
	/// Called when the entire dialogue has ended.
	/// </summary>
	/// <param name="node">The last node that has ended playing.</param>
	public void OnEnded(DialogueNode node);
}
