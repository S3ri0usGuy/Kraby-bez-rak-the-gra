using UnityEngine;

/// <summary>
/// An action that spends a specific number of minutes when activated.
/// </summary>
public class SpendTimeAction : MonoBehaviour
{
	[SerializeField]
	[Tooltip("How many minutes will be spent.")]
	private int _minutes = 15;

	/// <summary>
	/// Performs the action and spends the specified minutes.
	/// </summary>
	public void Perform()
	{
		Clock.instance.SpendMinutes(_minutes);
	}
}
