using System;

/// <summary>
/// A static class that provides safe arithmetic operations for 
/// signed 32-bit integers, preventing integer overflow.
/// </summary>
/// <remarks>
/// All operations clamp values to the 32-bit integer limits: 
/// <see cref="int.MinValue" /> and <see cref="int.MaxValue"/>.
/// </remarks>
public static class SafeInt32Math
{
	/// <summary>
	/// Adds two signed 32-bit integer numbers together preventing 
	/// integer overflow.
	/// </summary>
	/// <param name="left">The left signed 32-bit integer operand</param>
	/// <param name="right">The right signed 32-bit integer operand</param>
	/// <returns>
	/// A sum of two numbers clamped to the 
	/// [<see cref="int.MinValue" />, <see cref="int.MaxValue"/>] range.
	/// </returns>
	public static int SafeAdd(int left, int right)
	{
		// Prevents an integer overflow when sum is too large or too small
		long result = left + right;
		return (int)Math.Clamp(result, int.MinValue, int.MaxValue);
	}

	/// <summary>
	/// Multiplies a signed 32-bit integer by a 32-bit floating-point number.
	/// </summary>
	/// <param name="left">The left signed 32-bit integer operand.</param>
	/// <param name="right">The right 32-bit floating-point operand.</param>
	/// <returns>
	/// The rounded-down product of two numbers clamped to the 
	/// [<see cref="int.MinValue" />, <see cref="int.MaxValue"/>] range.
	/// </returns>
	public static int SafeMultiply(int left, float right)
	{
		// Not recursion - it calls the method with the same name
		// that uses double instead of float
		return SafeMultiply(left, (double)right);
	}

	/// <summary>
	/// Multiplies a signed 32-bit integer by a 64-bit floating-point number.
	/// </summary>
	/// <param name="left">The left signed 32-bit integer operand.</param>
	/// <param name="right">The right 64-bit floating-point operand.</param>
	/// <returns>
	/// The rounded-down product of two numbers clamped to the 
	/// [<see cref="int.MinValue" />, <see cref="int.MaxValue"/>] range.
	/// </returns>
	public static int SafeMultiply(int left, double right)
	{
		return (int)Math.Clamp(left * right, int.MinValue, int.MaxValue);
	}
}
