using System;

/// <summary>
/// A class that allows dynamic modification of a float value via multipliers and addends.
/// </summary>
[Serializable]
public sealed class ModifiableFloat : ModifiableProperty<float>
{
	public ModifiableFloat() : base(0f, float.NegativeInfinity, float.PositiveInfinity)
	{ }

	public ModifiableFloat(float value,
		float minValue = float.NegativeInfinity, float maxValue = float.PositiveInfinity) :
		base(value, minValue, maxValue)
	{ }

	protected override float Clamp(float value) => Math.Clamp(value, minValue, maxValue);
	protected override float Add(float left, float right) => left + right;
	protected override float Multiply(float left, float right) => left * right;
}