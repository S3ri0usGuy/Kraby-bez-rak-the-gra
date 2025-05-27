using UnityEngine;

public class SoundtrackZoneTriggerGroup : MonoBehaviour
{
	private int _count;

	[SerializeField]
	private SoundtrackZone _zone;

	public void Increment()
	{
		if (_count == 0)
		{
			SoundtrackController.instance.zone = _zone;
		}
		_count++;
	}

	public void Decrement()
	{
		_count--;
		if (_count < 0)
		{
			Debug.LogWarning($"SoundtrackZone ('{name}'): Tried to decrement from 0", gameObject);
			return;
		}
		if (_count == 0)
		{
			SoundtrackController.instance.zone = SoundtrackZone.None;
		}
	}
}
