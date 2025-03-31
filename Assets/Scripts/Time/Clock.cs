using System;
using UnityEngine;

/// <summary>
/// Represents a in-game clock that counts main protagonit's time left.
/// </summary>
public class Clock : SingletonMonoBehaviour<Clock>
{
	private int _minutesLeft = 60 * 24;

	[SerializeField]
	[Tooltip("How many minutes player is given at the start.")]
	private int _minutesAtStart = 60 * 24;

	/// <summary>
	/// Gets a number of minutes left.
	/// </summary>
	public int minutesLeft
	{
		get => _minutesLeft;
		private set
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
	/// An event that is triggered when the in-game time changes.
	/// </summary>
	public Action<Clock> timeUpdated;

	/// <summary>
	/// An event that is triggered when all minutes have been spent.
	/// </summary>
	public Action<Clock> timeOver;

	protected override void Awake()
	{
		base.Awake();
		_minutesLeft = _minutesAtStart;
	}

	public void SpendMinutes(int minutes)
	{
		if (_minutesLeft == 0)
		{
			Debug.LogWarning("Attempted to spend minutes when the time is over.");
			return;
		}

		if (minutes < minutesLeft)
		{
			Debug.LogWarning($"Requested to spend {minutes}, but only {minutesLeft} minutes are left.");
			minutesLeft = 0;
		}
		else
		{
			minutesLeft -= minutes;
		}
	}
}
