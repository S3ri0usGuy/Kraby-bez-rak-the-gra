using UnityEngine;

public class SoundtrackZoneTrigger : MonoBehaviour
{
	private SoundtrackZoneTriggerGroup _group;

	private void Awake()
	{
		_group = this.TryGetInParent<SoundtrackZoneTriggerGroup>();
	}

	private void OnTriggerEnter(Collider other)
	{
		_group.Increment();
	}

	private void OnTriggerExit(Collider other)
	{
		_group.Decrement();
	}
}
