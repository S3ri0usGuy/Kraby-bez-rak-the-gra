using UnityEngine;

/// <summary>
/// A component that checks the grounded state.
/// </summary>
public class GroundedChecker : MonoBehaviour
{
	private bool _isGrounded = true;
	public bool isGrounded => _isGrounded;

	[SerializeField]
	private float _radius = 0.495f;

	[SerializeField]
	private LayerMask _groundedLayerMask;

	private void OnEnable()
	{
		_isGrounded = true;
	}

	private void Update()
	{
		_isGrounded = Physics.CheckSphere(
			transform.position, _radius,
			_groundedLayerMask, QueryTriggerInteraction.Ignore);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, _radius);
	}
}
