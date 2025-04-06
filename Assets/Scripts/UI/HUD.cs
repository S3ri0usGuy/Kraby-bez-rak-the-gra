/// <summary>
/// A singleton that represents the object that contains main HUD components.
/// </summary>
public class HUD : SingletonMonoBehaviour<HUD>
{
	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
