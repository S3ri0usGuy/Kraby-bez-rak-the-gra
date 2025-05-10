using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component that provides subtitle displayers for the given subtitle profiles.
/// </summary>
public class SubtitlesProvider : SingletonMonoBehaviour<SubtitlesProvider>
{
	private Dictionary<SubtitlesProfile, SubtitlesDisplayer> _profileDisplayer = new();

	[SerializeField]
	[Tooltip("The subtitiles profile that is used by default.")]
	private SubtitlesProfile _defaultProfile;

	/// <summary>
	/// Gets the default profile.
	/// </summary>
	public SubtitlesProfile defaultProfile => _defaultProfile;

	/// <summary>
	/// Gets the subtitles displayer for the provided profile.
	/// </summary>
	/// <param name="profile">
	/// A profile for the subtitles. The default one will be used if it's
	/// <see langword="null"/>.
	/// </param>
	/// <returns>
	/// The subtitles displayer that corresponds to the <paramref name="profile" />.
	/// </returns>
	/// <exception cref="System.ArgumentException">
	/// The profile was not found.
	/// </exception>
	public SubtitlesDisplayer GetFor(SubtitlesProfile profile)
	{
		if (profile == null) profile = _defaultProfile;

		if (_profileDisplayer.TryGetValue(profile, out var displayer))
		{
			return displayer;
		}

		throw new System.ArgumentException($"The subtitiles profile " +
			$"\"{profile.name}\" has no displayer.", nameof(profile));
	}

	/// <summary>
	/// Registers the subtitles displayer corresponding to the profile.
	/// </summary>
	/// <param name="displayer">The displayer to be registered.</param>
	/// <exception cref="System.ArgumentNullException" />
	/// <exception cref="System.ArgumentException">
	/// The provided displayer has an invalid profile.
	/// </exception>
	public void Register(SubtitlesDisplayer displayer)
	{
		if (!displayer) throw new System.ArgumentNullException(nameof(displayer));
		if (!displayer.profile)
		{
			throw new System.ArgumentException($"The displayer " +
				$"\"{displayer.name}\" has no profile assigned to it.");
		}

		if (_profileDisplayer.TryGetValue(displayer.profile, out var existingDisplayer))
		{
			Debug.LogWarning("The already existing subtitles " +
				$"displayer (\"{existingDisplayer.name}\") was attempted to be overriden with" +
				$"\"{displayer.name}\".");
			return;
		}
	
		_profileDisplayer.Add(displayer.profile, displayer);
	}
}
