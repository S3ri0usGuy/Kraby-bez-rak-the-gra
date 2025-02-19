using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// A component that allows to display subtitles.
/// </summary>
public class SubtitlesDisplayer : SingletonMonoBehaviour<SubtitlesDisplayer>
{
	private bool _subtitleActive;
	private Coroutine _subtitleCoroutine;

	private SubtitlePriority _currentSubtitlePriority;

	[SerializeField]
	private TMP_Text _label;
	[SerializeField]
	[Tooltip("The lowest priority of the subtitle that can be shown. " +
		"Priorities lowest than this value will be ignored.")]
	private SubtitlePriority _priorityFilter = SubtitlePriority.Lowest;

	/// <summary>
	/// Gets/sets the lowest priority of the subtitle that can be shown.
	/// Priorities less important than this value will be ignored.
	/// </summary>
	public SubtitlePriority priorityFilter
	{
		get => _priorityFilter;
		set => _priorityFilter = value;
	}

	private IEnumerator ShowSubtitle(string subtitle, float duration)
	{
		_subtitleActive = true;
		_label.text = subtitle;
		_label.gameObject.SetActive(true);

		for (float t = 0f; t < duration; t += Time.deltaTime)
			yield return null;

		_label.gameObject.SetActive(false);
		_subtitleActive = false;
	}

	/// <summary>
	/// Tries to display a subtitle.
	/// </summary>
	/// <param name="subtitle">A subtitle to be displayed.</param>
	/// <param name="duration">A duration of the subtitle.</param>
	/// <param name="priority">
	/// A priority of the subtitle. The subtitles with the less important priority
	/// are overridden by the subtitles with the more important priority.
	/// </param>
	public void Display(string subtitle, float duration, SubtitlePriority priority)
	{
		// Apply subtitles filter
		if (priority > _priorityFilter)
			return;
		// Skip if the currently displayed subtitle is more important
		if (_subtitleActive && priority > _currentSubtitlePriority)
			return;

		if (_subtitleActive)
			StopCoroutine(_subtitleCoroutine);

		_currentSubtitlePriority = priority;
		_label.text = subtitle;

		_subtitleCoroutine = StartCoroutine(ShowSubtitle(subtitle, duration));
	}
}
