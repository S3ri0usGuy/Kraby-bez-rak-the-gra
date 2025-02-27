using UnityEngine;

/// <summary>
/// A component that visualizes a timer using the animator.
/// </summary>
[RequireComponent(typeof(Animator))]
public class ProgressTimer : MonoBehaviour
{
	private int _startTriggerId;
	private int _timeFloatId;

	private float _timeLeft;
	private float _duration;

	private Animator _animator;

	[SerializeField]
	private string _startTriggerName = "start";
	[SerializeField]
	private string _timeFloatName = "time";

	private void Awake()
	{
		_animator = GetComponent<Animator>();

		_startTriggerId = Animator.StringToHash(_startTriggerName);
		_timeFloatId = Animator.StringToHash(_timeFloatName);
	}

	private void Update()
	{
		if (_timeLeft > 0f)
		{
			_timeLeft -= Time.deltaTime;

			if (_timeLeft <= 0f)
			{
				// The time has passed
				_animator.SetFloat(_timeFloatId, 1f);
			}
			else
			{
				float normalizedTimeLeft = _timeLeft / _duration;
				_animator.SetFloat(_timeFloatId, 1f - normalizedTimeLeft);
			}
		}
	}

	/// <summary>
	/// Begins a countdown and the animation.
	/// </summary>
	public void StartCountdown(float duration)
	{
		_duration = _timeLeft = duration;
		_animator.SetTrigger(_startTriggerId);
	}
}
