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
		int hours = _clock.minutesLeft / 60;
		int minutes = _clock.minutesLeft % 60;
		_label.text = $"{hours}:{minutes}";
	}

	private void OnUpdated(Clock clock)
	{
		UpdateTime();
	}
}
