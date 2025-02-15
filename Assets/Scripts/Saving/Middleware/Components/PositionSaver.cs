/// <summary>
/// A component that saves a position and rotation of the current object.
/// </summary>
public class PositionSaver : SavableComponent<PositionSaveData>
{
	protected override PositionSaveData fallbackData => new(transform.position, transform.rotation);

	protected override void OnLoad()
	{
		data.Deconstruct(out var position, out var rotation);
		transform.SetPositionAndRotation(position, rotation);
	}

	protected override void OnSave()
	{
		data.Update(transform.position, transform.rotation);
	}
}
