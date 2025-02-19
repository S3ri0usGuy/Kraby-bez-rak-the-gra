using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// A component that allows to display subtitles.
/// </summary>
public class SubtitlesDisplayer : SingletonMonoBehaviour<SubtitlesDisplayer>
{
	private bool _subtitleActive = false;
	private Coroutine _subtitleCoroutine;

	private SubtitlePriority _currentSubtitlePriority;

	[SerializeField]
	private TMP_Text _label;
	[SerializeField]
	[Tooltip("The lowest priority of the subtitle that can be shown. " +
		"Priorities lowest than this value will be ignored.")]
	private SubtitlePriority _priorityFilter = SubtitlePriority.Lowest;

	[SerializeField]
	[Tooltip("A format that defines how speaker name is displayed. '{0}' represents " +
		"the speaker's name")]
	private string _speakerNameFormat = "<b>{0}</b>: ";

	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("A ratio that defines how long it takes for the typing animation. " +
		"For example, 50% means that all symbols are printed after a half of " +
		"the phrase duration had passed, and 0% means that all symbols are printed " +
		"immediately.")]
	private float _typeAnimationRatio = 0.8f;

	/// <summary>
	/// Gets/sets the lowest priority of the subtitle that can be shown.
	/// Priorities less important than this value will be ignored.
	/// </summary>
	public SubtitlePriority priorityFilter
	{
		get => _priorityFilter;
		set => _priorityFilter = value;
	}

	private IEnumerator ShowSubtitle(string subtitlePrefix, string subtitle, float duration)
	{
		_subtitleActive = true;
		_label.text = subtitlePrefix;
		_label.gameObject.SetActive(true);

		float typingDuration = duration * _typeAnimationRatio;
		float symbolDuration = typingDuration / subtitle.Length;

		for (int i = 0; i < subtitle.Length; i++)
		{
			_label.text += subtitle[i];
			yield return new WaitForSeconds(symbolDuration);
		}

		yield return new WaitForSeconds(duration - typingDuration);

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
	/// <param name="speaker">An optional speaker's name.</param>
	public void Display(string subtitle, float duration, SubtitlePriority priority,
		string speaker = null)
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

		string subtitlePrefix = string.IsNullOrEmpty(speaker) ?
			"" : string.Format(_speakerNameFormat, speaker);

		_subtitleCoroutine = StartCoroutine(ShowSubtitle(subtitlePrefix, subtitle, duration));
	}
}
