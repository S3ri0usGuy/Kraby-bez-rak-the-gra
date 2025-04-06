using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : Trigger
{
	[SerializeField]
	private UnityEvent _playerEntered;

	private void OnTriggerEnter(Collider other)
	{
		_playerEntered.Invoke();
		InvokeTriggered();
	}
}
