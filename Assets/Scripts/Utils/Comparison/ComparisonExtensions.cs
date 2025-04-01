using System;
using UnityEngine;

public static class ComparisonExtensions
{
	/// <summary>
	/// Compares a value to the target.
	/// </summary>
	/// <param name="value">The value to compare.</param>
	/// <param name="target">The target to compare the value with.</param>
	/// <returns>A result of comparison.</returns>
	/// <exception cref="ArgumentException">Thrown when comparison option cannot be identified.</exception>
	public static bool Compare(this Comparison comparison, float value, float target)
	{
		return comparison switch
		{
			Comparison.Less => value < target,
			Comparison.LessOrEqual => value <= target,
			Comparison.Greater => value > target,
			Comparison.GreaterOrEqual => value >= target,
			Comparison.Equal => Mathf.Approximately(value, target),
			Comparison.NotEqual => !Mathf.Approximately(value, target),
			_ => throw new ArgumentException("Invalid comparison option.", nameof(comparison))
		};
	}

	/// <summary>
	/// Compares a value to the target.
	/// </summary>
	/// <param name="value">The value to compare.</param>
	/// <param name="target">The target to compare the value with.</param>
	/// <returns>A result of comparison.</returns>
	/// <exception cref="ArgumentException">Thrown when comparison option cannot be identified.</exception>
	public static bool Compare(this Comparison comparison, int value, int target)
	{
		return comparison switch
		{
			Comparison.Less => value < target,
			Comparison.LessOrEqual => value <= target,
			Comparison.Greater => value > target,
			Comparison.GreaterOrEqual => value >= target,
			Comparison.Equal => value == target,
			Comparison.NotEqual => value != target,
			_ => throw new ArgumentException("Invalid comparison option.", nameof(comparison))
		};
	}
}
