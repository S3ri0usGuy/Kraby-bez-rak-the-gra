/// <summary>
/// A context for the dialogue phrase callback.
/// </summary>
public class DialoguePhraseContext
{
	/// <summary>
	/// A node of the dialogue.
	/// </summary>
	public DialogueNode node { get; }
	/// <summary>
	/// A phrase related to the callback.
	/// </summary>
	public DialoguePhrase phrase { get; }
	/// <summary>
	/// An index of the <see cref="phrase" />.
	/// </summary>
	public int phraseIndex { get; }

	public DialoguePhraseContext(DialogueNode node, DialoguePhrase phrase, int phraseIndex)
	{
		this.node = node;
		this.phrase = phrase;
		this.phraseIndex = phraseIndex;
	}
}
