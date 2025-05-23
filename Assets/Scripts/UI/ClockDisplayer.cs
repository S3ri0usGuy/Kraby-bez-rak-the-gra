using TMPro;
using UnityEngine;

/// <summary>
/// A component that shows time left.
/// </summary>
public class ClockDisplayer : MonoBehaviour
{
	private Clock _clock;

	[SerializeField]
	private TMP_Text _label;

	[SerializeField, Min(0)]
	private int _startTimeMinutes = 7 * 60;

	private void Start()
	{
		if (Clock.exists)
		{
			_clock = Clock.instance;
			_clock.timeUpdated += OnUpdated;

			UpdateTime();
		}
	}

	private void UpdateTime()
	{
		int totalMinutes = _clock.minutesAtStart - _clock.minutesLeft + _startTimeMinutes;

		int hours = totalMinutes / 60;
		int minutes = totalMinutes % 60;
		_label.text = $"{hours:00}:{minutes:00}";
	}

	private void OnUpdated(Clock clock)
	{
		UpdateTime();
	}
}
