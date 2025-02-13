using System;
using UnityEngine;

/// <summary>
/// A component which interacts with the <see cref="SaveSystem" /> singleton and
/// allows saving.
/// </summary>
/// <typeparam name="TData">
/// A type of the saved data. Must be a reference type and be serializable.
/// </typeparam>
public abstract class SavableComponent<TData> : MonoBehaviour, ISavable
	where TData : class
{
	private TData _data;

	[HideInInspector, SerializeField]
	private Guid _guid = Guid.NewGuid();

	public Guid id => _guid;

	/// <summary>
	/// Gets a data that is used when the data retrieval had failed.
	/// </summary>
	/// <remarks>
	/// Returns <see langword="null" /> when not overriden.
	/// </remarks>
	protected virtual TData fallbackData => null;

	/// <summary>
	/// Gets a savable data assigned to this component.
	/// </summary>
	public TData data { get; private set; }

	protected virtual void Awake()
	{
		if (!SaveSystem.exists)
		{
			Debug.LogError($"The savable component does not have an access to the save system. " +
				$"Make sure that the {nameof(SaveSystem)} component is present on the scene.");
			return;
		}

		SaveSystem.instance.AddSavable(this);
	}

	/// <summary>
	/// A method that is called after loading and before saving
	/// to ensure that data is valid. 
	/// </summary>
	/// <remarks>
	/// When implemented, it must
	/// </remarks>
	/// <param name="data">
	/// The data to validate, cannot be <see cref="null" />.
	/// </param>
	protected virtual void Validate(TData data) { }

	/// <summary>
	/// A method that is called after the data is loaded.
	/// </summary>
	protected virtual void OnLoad() { }

	/// <summary>
	/// A method that is called before the data is saved.
	/// </summary>
	protected virtual void OnSave() { }

	public void Load(object data)
	{
		if (data == null)
		{
			_data = null;
			OnLoad();
			return;
		}

		if (data is not TData unboxedData)
		{
			_data = null;

			Debug.LogWarning(
				$"The data was expected to have a type '{typeof(TData).Name}', " +
				$"but was '{data.GetType().Name}' instead.\n" +
				$"Save GUID: {_guid}.",
				gameObject);

			OnLoad();
			return;
		}

		_data = unboxedData;
		OnLoad();
	}

	public object Save()
	{
		OnSave();
		return _data;
	}
}
