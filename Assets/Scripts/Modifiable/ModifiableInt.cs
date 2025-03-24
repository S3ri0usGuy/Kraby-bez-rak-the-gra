using System;

/// <summary>
/// A class that allows dynamic modification of an integer value via multipliers and addends.
/// </summary>
[Serializable]
public sealed class ModifiableInt : ModifiableProperty<int>
{
	public ModifiableInt() : base(0, int.MinValue, int.MaxValue)
	{ }

	public ModifiableInt(int value,
		int minValue = int.MinValue, int maxValue = int.MaxValue) :
		base(value, minValue, maxValue)
	{ }

	protected override int Clamp(int value) => Math.Clamp(value, minValue, maxValue);
	protected override int Add(int left, int right)
	{
		return SafeInt32Math.SafeAdd(left, right);
	}
	protected override int Multiply(int left, float right)
	{
		return SafeInt32Math.SafeMultiply(left, right);
	}
}
