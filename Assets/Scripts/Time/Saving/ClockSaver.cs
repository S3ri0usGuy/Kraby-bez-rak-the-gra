using UnityEngine;

[RequireComponent(typeof(Clock))]
public class ClockSaver : SavableComponent<ClockSaveData>
{
	private Clock _clock;

	protected override ClockSaveData fallbackData => new()
	{
		minutesLeft = _clock.minutesAtStart
	};

	protected override void Awake()
	{
		_clock = GetComponent<Clock>();
		base.Awake();
	}

	protected override void Validate(ClockSaveData data)
	{
		if (data.minutesLeft < 0)
		{
			Debug.LogWarning($"A negative number of minutes left has been encountered: {data.minutesLeft}.");
			data.minutesLeft = 0;
		}
		else if (data.minutesLeft > _clock.minutesAtStart)
		{
			Debug.LogWarning($"An unexpected number of minutes left has been encountered: " +
				$"{data.minutesLeft}. The minutes limit is {_clock.minutesAtStart}.");
			data.minutesLeft = _clock.minutesAtStart;
		}
	}

	protected override void OnLoad()
	{
		_clock.minutesLeft = data.minutesLeft;
	}

	protected override void OnSave()
	{
		data.minutesLeft = _clock.minutesLeft;
	}
}
