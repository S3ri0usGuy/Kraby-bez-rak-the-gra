using UnityEngine;

public static class ComponentExtensions
{
	private static void ValidateComponent<TComponent>(Component component)
		where TComponent : Component
	{
		if (component == null)
		{
			Debug.LogError($"Could not found the " +
				$"{typeof(TComponent).Name} component.", component);
		}
	}

	/// <summary>
	/// Tries to get a component in children and logs an error
	/// when it has not been found.
	/// </summary>
	/// <typeparam name="TComponent">A type of the component to find.</typeparam>
	/// <returns>
	/// A component of the <typeparamref name="TComponent"/> type.
	/// </returns>
	public static TComponent TryGetInChildren<TComponent>(this Component component, bool inclueInactive = true)
		where TComponent : Component
	{
		var childrenComponent = component.GetComponentInChildren<TComponent>(inclueInactive);
		ValidateComponent<TComponent>(childrenComponent);
		return childrenComponent;
	}

	/// <summary>
	/// Tries to get a component in parent and logs an error
	/// when it has not been found.
	/// </summary>
	/// <typeparam name="TComponent">A type of the component to find.</typeparam>
	/// <returns>
	/// A component of the <typeparamref name="TComponent"/> type.
	/// </returns>
	public static TComponent TryGetInParent<TComponent>(this Component component, bool includeInactive = true)
		where TComponent : Component
	{
		var childrenComponent = component.GetComponentInParent<TComponent>(includeInactive);
		ValidateComponent<TComponent>(childrenComponent);
		return childrenComponent;
	}
}
