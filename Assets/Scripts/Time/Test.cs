using UnityEngine;

public class Test : MonoBehaviour
{
	[SerializeField]
	private int _minutes = 5;

	private void OnEnable()
	{
		Clock.instance.SpendMinutes(_minutes);
	}
}
