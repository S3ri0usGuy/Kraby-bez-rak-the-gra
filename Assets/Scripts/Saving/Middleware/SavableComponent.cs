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
	private bool _isLoading;

	[SerializeField]
	[Tooltip("A unique identifier that represents this object.")]
	private string _id = Guid.NewGuid().ToString();

	public string id => _id;

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
	public TData data
	{
		get => _data;
		protected set => _data = value;
	}

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

	protected virtual void OnDestroy()
	{
		if (SaveSystem.exists) // Prevents a bug which happens when the game is exited
		{
			SaveSystem.instance.RemoveSavable(this);
		}
	}

	/// <summary>
	/// A method that is called after loading and before saving
	/// to ensure that data is valid. 
	/// </summary>
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

	/// <summary>
	/// Requests the auto save. Doesn't do anything if called while loading.
	/// </summary>
	protected void RequestAutoSave()
	{
		if (!_isLoading) SaveSystem.RequestAutoSave();
	}

	public void Load(object data)
	{
		data ??= fallbackData;
		_isLoading = true;

		if (data is TData unboxedData)
		{
			Validate(unboxedData);
			_data = unboxedData;
		}
		else
		{
			_data = fallbackData;

			Debug.LogWarning(
				$"The data was expected to have a type '{typeof(TData).Name}', " +
				$"but was '{data.GetType().Name}' instead.\n" +
				$"Save string: {_id}.",
				gameObject);
		}

		OnLoad();
		_isLoading = false;
	}

	public object Save()
	{
		OnSave();
		Validate(_data);
		return _data;
	}
}
