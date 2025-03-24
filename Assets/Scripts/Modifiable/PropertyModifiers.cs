using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents a collection of property's modifiers.
/// </summary>
/// <typeparam name="T">Type of the property modifiers.</typeparam>
public sealed class PropertyModifiers<T> : IEnumerable<T>
{
	private int _nextId = 0;

	private readonly Dictionary<int, T> _modifiers = new();

	/// <summary>
	/// Event that is called when any change to the modifiers is perfomed.
	/// </summary>
	public event Action updated;

	/// <summary>
	/// Adds a modifier to the collection.
	/// </summary>
	/// <param name="modifier">Modifier value.</param>
	/// <returns>ID of the modifier which can be then used for the removal.</returns>
	public int Add(T modifier)
	{
		int id = _nextId++;
		_modifiers[id] = modifier;
		updated?.Invoke();
		return id;
	}

	/// <summary>
	/// Remove a modifier by its ID.
	/// </summary>
	/// <param name="modifierID">ID of the modifier.</param>
	/// <exception cref="ArgumentException">Invalid ID of the modifier.</exception>
	public void Remove(int modifierID)
	{
		if (!_modifiers.Remove(modifierID))
		{
			throw new ArgumentException("Invalid ID used for the modifier removal operation.", nameof(modifierID));
		}
		updated?.Invoke();
	}

	/// <summary>
	/// Removes all modifiers.
	/// </summary>
	public void Clear()
	{
		_modifiers.Clear();
		updated?.Invoke();
	}

	/// <summary>
	/// Accesses a modifier by its ID.
	/// </summary>
	/// <param name="modifierId">ID of the modifier.</param>
	/// <returns>A modifier which is assigned to the passed ID.</returns>
	public T this[int modifierId]
	{
		get => _modifiers[modifierId];
		set
		{
			_modifiers[modifierId] = value;
			updated?.Invoke();
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		return _modifiers.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _modifiers.Values.GetEnumerator();
	}
}
