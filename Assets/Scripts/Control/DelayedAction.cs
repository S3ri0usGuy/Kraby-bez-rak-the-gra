using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DelayedAction : MonoBehaviour
{
	[SerializeField, Min(0f)]
	[Tooltip("Delay in seconds.")]
	private float _delay = 5f;

	[SerializeField]
	private UnityEvent _action;

	private IEnumerator DelayedInvoke()
	{
		yield return new WaitForSeconds(_delay);
		_action.Invoke();
	}

	public void Perform()
	{
		StartCoroutine(DelayedInvoke());
	}
}
