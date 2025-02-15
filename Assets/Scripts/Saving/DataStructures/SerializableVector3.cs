using UnityEngine;

[System.Serializable]
public struct SerializableVector3
{
	public float x { get; set; }
	public float y { get; set; }
	public float z { get; set; }

	public SerializableVector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public SerializableVector3(Vector3 vector)
	{
		x = vector.x;
		y = vector.y;
		z = vector.z;
	}

	public static implicit operator Vector3(SerializableVector3 v) => new(v.x, v.y, v.z);
	public static implicit operator SerializableVector3(Vector3 v) => new(v);
}
