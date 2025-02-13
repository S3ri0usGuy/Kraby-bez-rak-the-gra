using System;
using UnityEngine;

[Serializable]
public class PositionSaveData : MonoBehaviour
{
	public Vector3 position { get; set; }
	public Quaternion rotation { get; set; }
}

