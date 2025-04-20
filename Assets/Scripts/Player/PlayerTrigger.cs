using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : Trigger
{
	[SerializeField]
	private UnityEvent _playerEntered;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<Player>(out _))
		{
			_playerEntered.Invoke();
			InvokeTriggered();
		}
	}
}
