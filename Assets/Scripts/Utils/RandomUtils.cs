using UnityEngine;

/// <summary>
/// A class which provides <see cref="UnityEngine.Random" /> utilities.
/// </summary>
public static class RandomUtils
{
	/// <summary>
	/// Returns a random boolean.
	/// </summary>
	/// <param name="trueProbability">Probability of returning true. Should be in range from 0 to 1.</param>
	/// <returns>
	/// A random boolean.
	/// </returns>
	public static bool Bool(float trueProbability = 0.5f)
	{
		return trueProbability >= 1f || Random.value < trueProbability;
	}

	/// <summary>
	/// Returns a random point on a circle with a specific radius in XZ coordinates.
	/// </summary>
	/// <remarks>
	/// This method is implemented, because UnityEngine.Random does not provide
	/// a property onUnitCircle, and normalizing a <see cref="Random.insideUnitCircle" /> is bad, because
	/// theoretically this property can be a zero vector.
	/// </remarks>
	/// <param name="radius">Radius of a circle.</param>
	/// <returns>A random point on a circle in XZ coordinates (y is always 0).</returns>
	public static Vector3 OnCircle(float radius)
	{
		float angle = Random.value * 2f * Mathf.PI;

		float x = Mathf.Cos(angle) * radius;
		float z = Mathf.Sin(angle) * radius;

		return new(x, 0f, z);
	}
}
