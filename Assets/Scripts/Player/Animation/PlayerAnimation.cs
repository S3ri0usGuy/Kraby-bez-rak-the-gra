using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
	private Animator _animator;

	[SerializeField]
	private PlayerMovement _movement;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		_animator.SetBool("isWalking", _movement.currentSpeed > 0.1f);
	}
}
