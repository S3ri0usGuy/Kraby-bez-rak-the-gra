using System;
using UnityEngine;

/// <summary>
/// An abstract class that helps to ensure that only one instance of a derived class exists at any time
/// on a scene.
/// </summary>
/// <typeparam name="T">Type of the derived class.</typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	private static T _instance;

	/// <summary>
	/// Gets a current instance of the object. Throws an exception if there is no instance.
	/// </summary>
	public static T instance
	{
		get
		{
			if (!_instance)
				throw new InvalidOperationException($"There is no instance of the singleton type {typeof(T)}.");

			return _instance;
		}
	}

	/// <summary>
	/// Gets a flag indicating whether there is an instance of the singleton on the current scene.
	/// </summary>
	public static bool exists => _instance;

	protected virtual void Awake()
	{
		if (_instance)
		{
			Debug.LogWarning($"Unexpected multiple instances of the singleton type {typeof(T)}.");
		}

		// Assign the current instance
		_instance = (T)this;
	}

	protected virtual void OnDestroy()
	{
		// Set the instance to null to indicate that the singleton instance is no longer available
		_instance = null;
	}
}
