using UnityEngine;

/// <summary>
/// A singleton that provides dialogue save event keys.
/// </summary>
[RequireComponent(typeof(DialogueSaveEventsStorage))]
public sealed class DialogueSaveEventsProvider : SingletonMonoBehaviour<DialogueSaveEventsProvider>
{
	private DialogueSaveEventsStorage _storage;

	protected override void Awake()
	{
		base.Awake();

		_storage = GetComponent<DialogueSaveEventsStorage>();
	}

	/// <summary>
	/// Checks whether the key exists.
	/// </summary>
	/// <param name="key">The key which is checked for existance.</param>
	/// <returns>
	/// <see langword="true" /> if the <paramref name="key"/> exists,
	/// otherwise <see langword="false" />.
	/// </returns>
	/// <exception cref="System.ArgumentNullException" />
	public bool ContainsKey(string key)
	{
		if (key == null)
			throw new System.ArgumentNullException(nameof(key));

		return _storage.keys.Contains(key);
	}

	/// <summary>
	/// Adds the dialogue event save key.
	/// </summary>
	/// <param name="key">A key to be added.</param>
	/// <exception cref="System.ArgumentNullException" />
	public void AddKey(string key)
	{
		if (key == null)
			throw new System.ArgumentNullException(nameof(key));

		_storage.keys.Add(key);
	}

	/// <summary>
	/// Removes the dialogue event save key.
	/// </summary>
	/// <remarks>
	/// Does nothing if the key doesn't exist.
	/// </remarks>
	/// <param name="key">A key to be removed.</param>
	/// <exception cref="System.ArgumentNullException" />
	public void RemoveKey(string key)
	{
		if (key == null)
			throw new System.ArgumentNullException(nameof(key));

		_storage.keys.Remove(key);
	}
}
