using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// A component that allows to display subtitles.
/// </summary>
public class SubtitlesDisplayer : MonoBehaviour
{
	private bool _isTyping = false;
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

	[SerializeField]
	private SubtitlesProfile _profile;

	/// <summary>
	/// Gets/sets the lowest priority of the subtitle that can be shown.
	/// Priorities less important than this value will be ignored.
	/// </summary>
	public SubtitlePriority priorityFilter
	{
		get => _priorityFilter;
		set => _priorityFilter = value;
	}

	/// <summary>
	/// Gets the subtitiles profile associated with the displayer.
	/// </summary>
	public SubtitlesProfile profile => _profile;

	/// <summary>
	/// Gets a flag indicating whether the subtitiles currently in the
	/// typing animation state.
	/// </summary>
	public bool isTyping => _isTyping;

	private void Awake()
	{
		SubtitlesProvider.instance.Register(this);
	}

	private IEnumerator ShowSubtitle(string subtitlePrefix, string subtitle, float duration)
	{
		_subtitleActive = true;
		_label.text = subtitlePrefix;
		_label.gameObject.SetActive(true);

		float typingDuration = duration * _typeAnimationRatio;
		float symbolDuration = typingDuration / subtitle.Length;

		float t = 0f;
		_isTyping = true;
		for (int i = 0; i < subtitle.Length && _isTyping; i++)
		{
			_label.text += subtitle[i];
			while (_isTyping && t < symbolDuration)
			{
				t += Time.deltaTime;
				duration -= Time.deltaTime;
				yield return null;
			}
			t -= symbolDuration;
		}
		_isTyping = false;
		_label.text = subtitlePrefix + subtitle;

		yield return new WaitForSeconds(duration);

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

	/// <summary>
	/// Skips the typing animation if it's active.
	/// </summary>
	public void SkipTyping()
	{
		_isTyping = false;
	}

	/// <summary>
	/// Cancels the currently shown subtitle.
	/// </summary>
	public void Cancel()
	{
		if (_subtitleActive)
			StopCoroutine(_subtitleCoroutine);

		_label.gameObject.SetActive(false);
		_subtitleActive = false;
	}
}
