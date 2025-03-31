/// <summary>
/// A singleton that represents the player and provides references
/// to the components related to the player.
/// </summary>
public class Player : SingletonMonoBehaviour<Player>
{
	public DialogueSpeaker speaker { get; private set; }
	public PlayerRotation rotation { get; private set; }
	public PlayerMovement movement { get; private set; }

	protected override void Awake()
	{
		base.Awake();

		speaker = this.TryGetInChildren<DialogueSpeaker>();
		rotation = this.TryGetInChildren<PlayerRotation>();
		movement = this.TryGetInChildren<PlayerMovement>();
	}
}
