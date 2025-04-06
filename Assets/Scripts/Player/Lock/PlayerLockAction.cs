using UnityEngine;

/// <summary>
/// A component that allows to lock/unlock the plyaer.
/// </summary>
public class PlayerLockAction : MonoBehaviour
{
	public void Lock()
	{
		PlayerLocker.Lock();
	}

	public void Unlock()
	{
		PlayerLocker.Unlock();
	}
}
