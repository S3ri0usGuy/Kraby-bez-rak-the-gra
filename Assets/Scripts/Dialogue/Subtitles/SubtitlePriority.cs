/// <summary>
/// An enum that defines subtitle priorities. The lower the value, the
/// more important the subtitle is.
/// </summary>
public enum SubtitlePriority
{
	/// <summary>
	/// A priority used for dialogues. The highest subtitle priority.
	/// </summary>
	Dialogue = int.MinValue,
	/// <summary>
	/// A priority for important phrases that are no dialogues.
	/// </summary>
	Important = 0,
	/// <summary>
	/// A priority for phrases that don't matter that much and can
	/// be skipped.
	/// </summary>
	Optional = 100,
	/// <summary>
	/// A priority for the background phrases (e.g. for voices that
	/// are very far away).
	/// </summary>
	Background = 200,
	/// <summary>
	/// The lowest priority. Useful to specify that all subtitles
	/// must be displayed in the <see cref="SubtitlesDisplayer" />.
	/// </summary>
	Lowest = int.MaxValue
}
