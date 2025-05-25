using System;
using UnityEngine;

/// <summary>
/// Represents a in-game clock that counts main protagonit's time left.
/// </summary>
public class Clock : SingletonMonoBehaviour<Clock>
{
	private int _minutesLeft = -1;

	[SerializeField]
	[Tooltip("How many minutes player is given at the start.")]
	private int _minutesAtStart = 60 * 24;
	[SerializeField]
	[Tooltip("Time when the game starts.")]
	private int _startTimeMinutes = 7 * 60;

	/// <summary>
	/// Gets/sets a number of minutes left.
	/// </summary>
	/// <remarks>
	/// Please, consider using the <see cref="SpendMinutes(int)" /> method instead
	/// of directly setting this property unless you know what you are doing.
	/// </remarks>
	public int minutesLeft
	{
		get => _minutesLeft;
		set
		{
			if (value <= 0)
			{
				_minutesLeft = 0;
				timeUpdated?.Invoke(this);
				timeOver?.Invoke(this);
			}
			else
			{
				_minutesLeft = value;
				timeUpdated?.Invoke(this);
			}
		}
	}
	/// <summary>
	/// Gets the time in minutes when the game starts.
	/// </summary>
	public int startTimeMinutes => _startTimeMinutes;

	/// <summary>
	/// Gets a number of minutes that player is given at the start.
	/// </summary>
	public int minutesAtStart => _minutesAtStart;

	/// <summary>
	/// An event that is triggered when the in-game time changes.
	/// </summary>
	public event Action<Clock> timeUpdated;

	/// <summary>
	/// An event that is triggered when all minutes have been spent.
	/// </summary>
	public event Action<Clock> timeOver;

	protected override void Awake()
	{
		base.Awake();
		if (_minutesLeft == -1) _minutesLeft = _minutesAtStart;
	}

	public void SpendMinutes(int minutes)
	{
		if (_minutesLeft == 0)
		{
			Debug.LogWarning("Attempted to spend minutes when the time is over.");
			return;
		}

		minutesLeft -= minutes;
	}
}
