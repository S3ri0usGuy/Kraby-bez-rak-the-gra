using System;
using UnityEngine;

[Serializable]
public class PositionSaveData
{
	private Vector3 _position;
	private Quaternion _rotation;

	public SerializableVector3 position
	{
		get => (SerializableVector3)_position;
		set => _position = (Vector3)value;
	}

	public SerializableVector3 rotation
	{
		get => (SerializableVector3)_rotation.eulerAngles;
		set => _rotation = Quaternion.Euler((Vector3)value);
	}

	public PositionSaveData() { }

	public PositionSaveData(Vector3 position, Quaternion rotation)
	{
		_position = position;
		_rotation = rotation;
	}

	public void Update(Vector3 position, Quaternion rotation)
	{
		_position = position;
		_rotation = rotation;
	}

	public void Deconstruct(out Vector3 position, out Quaternion rotation)
	{
		position = _position;
		rotation = _rotation;
	}
}

