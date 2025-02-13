/// <summary>
/// A component that saves a position and rotation of the current object.
/// </summary>
public class PositionSaver : SavableComponent<PositionSaveData>
{
	protected override PositionSaveData fallbackData => new()
	{
		position = transform.position,
		rotation = transform.rotation
	};

	protected override void OnLoad()
	{
		transform.SetPositionAndRotation(data.position, data.rotation);
	}

	protected override void OnSave()
	{
		data.position = transform.position;
		data.rotation = transform.rotation;
	}
}
