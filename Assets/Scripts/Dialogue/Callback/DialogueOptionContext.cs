/// <summary>
/// A context for the dialogue option callback.
/// </summary>
public class DialogueOptionContext
{
	/// <summary>
	/// A node of the dialogue.
	/// </summary>
	public DialogueNode node { get; }
	/// <summary>
	/// An option related to the callback.
	/// </summary>
	public DialogueOption option { get; }
	/// <summary>
	/// An index of the <see cref="option" />.
	/// </summary>
	public int optionIndex { get; }

	public DialogueOptionContext(DialogueNode node, DialogueOption option, int optionIndex)
	{
		this.node = node;
		this.option = option;
		this.optionIndex = optionIndex;
	}
}
