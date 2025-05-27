using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// The component that controls the soundtrack.
/// </summary>
public class SoundtrackController : SingletonMonoBehaviour<SoundtrackController>
{
	private bool _muffled;
	private bool _stopped;
	private SoundtrackZone _zone = SoundtrackZone.None;

	[SerializeField]
	private AudioMixer _mixer;

	[SerializeField]
	private AudioMixerSnapshot _defaultSnapshot;
	[SerializeField]
	private AudioMixerSnapshot _muffledSnapshot;
	[SerializeField]
	private AudioMixerSnapshot _noSoundtrackSnapshot;
	[SerializeField]
	private AudioMixerSnapshot _townSnapshot;
	[SerializeField]
	private AudioMixerSnapshot _churchSnapshot;

	[SerializeField]
	private float _transitionTime = 1.0f;

	public bool muffled
	{
		get => _muffled;
		set
		{
			_muffled = value;
			UpdateSnapshot();
		}
	}
	public bool stopped
	{
		get => _stopped;
		set
		{
			_stopped = value;
			UpdateSnapshot();
		}
	}
	public SoundtrackZone zone
	{
		get => _zone;
		set
		{
			_zone = value;
			UpdateSnapshot();
		}
	}

	private void UpdateSnapshot()
	{
		var snapshots = new AudioMixerSnapshot[]
		{
			_defaultSnapshot, _muffledSnapshot, _noSoundtrackSnapshot, _townSnapshot, _churchSnapshot
		};

		float[] weights = new float[5];

		if (_stopped)
		{
			weights[2] = 1f; // Only NoSoundtrack
		}
		else
		{
			float zoneWeight = 1f;
			if (_muffled)
			{
				zoneWeight = 0.5f;
				weights[1] = 1f; // Muffled
			}

			switch (_zone)
			{
				case SoundtrackZone.Town:
					weights[3] = zoneWeight;
					break;
				case SoundtrackZone.Church:
					weights[4] = zoneWeight;
					break;
				default:
					weights[0] = 1f;
					break;
			}
		}

		_mixer.TransitionToSnapshots(snapshots, weights, _transitionTime);
	}
}
