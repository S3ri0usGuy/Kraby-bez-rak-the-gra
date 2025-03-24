using System;
using UnityEngine;

/// <summary>
/// A class that allows dynamic modification of a numeric value via multipliers and addends.
/// </summary>
/// <remarks>
/// Don't use this class directly, instead use: <seealso cref="ModifiableFloat"/> or
/// <seealso cref="ModifiableInt"/>.
/// </remarks>
/// <typeparam name="T">Type of the property.</typeparam>
[Serializable]
public abstract class ModifiableProperty<T> : ISerializationCallbackReceiver
{
	[SerializeField]
	private T _baseValue = default;
	protected readonly T minValue, maxValue;

	// Value after applying all modifiers
	[SerializeField]
	private T _actualValue = default;

	/// <summary>
	/// Values which are added to the base value.
	/// </summary>
	public PropertyModifiers<T> addends { get; } = new();
	/// <summary>
	/// Values which are multiplied to the value after applying addends.
	/// </summary>
	public PropertyModifiers<float> multipliers { get; } = new();

	// Unfortunately, using + and * operators with generics is not possible in C#,
	// so we will use these abstract methods instead.
	protected abstract T Clamp(T value);
	protected abstract T Add(T left, T right);
	protected abstract T Multiply(T left, float right);

	/// <summary>
	/// Gets/sets a base value, modifiers are not applied.
	/// </summary>
	public T baseValue
	{
		get => _baseValue;
		set
		{
			_baseValue = Clamp(value);
			UpdateValue();
		}
	}

	/// <summary>
	/// Gets a value obtained after applying all modifiers.
	/// </summary>
	public T actualValue => _actualValue;

	/// <summary>
	/// Event that is called when value or any of the modifiers is changed.
	/// </summary>
	public event Action<T> valueChanged;

	private void UpdateValue()
	{
		_actualValue = Clamp(ApplyModifiers(baseValue));

		valueChanged?.Invoke(_actualValue);
	}

	protected ModifiableProperty(T value, T minValue, T maxValue)
	{
		this.minValue = minValue;
		this.maxValue = maxValue;
		baseValue = value;

		addends.updated += UpdateValue;
		multipliers.updated += UpdateValue;

		UpdateValue();
	}

	/// <summary>
	/// Applies all modifiers to the <paramref name="value"/> without touching the base value.
	/// </summary>
	/// <remarks>
	/// Min and max value parameters are ignored.
	/// </remarks>
	/// <param name="value">A value to apply modifiers to.</param>
	/// <returns>A <paramref name="value"/> after applying all modifiers.</returns>
	public T ApplyModifiers(T value)
	{
		foreach (T addend in addends)
		{
			value = Add(value, addend);
		}
		float multipliersProduct = 1f;
		foreach (float multiplier in multipliers)
		{
			multipliersProduct *= multiplier;
		}
		return Multiply(value, multipliersProduct);
	}

	/// <summary>
	/// Resets all modifiers, doesn't touch the base value.
	/// </summary>
	public void Reset()
	{
		addends.Clear();
		multipliers.Clear();
	}

	// Enforces limits in editor
	public void OnBeforeSerialize()
	{
		_baseValue = Clamp(_baseValue);
	}
	public void OnAfterDeserialize()
	{
		_actualValue = _baseValue = Clamp(_baseValue);
	}

	public override string ToString()
	{
		return $"{{baseValue: {_baseValue}, actualValue: {_actualValue}}}";
	}

	public static implicit operator T(ModifiableProperty<T> property) => property.actualValue;
}
