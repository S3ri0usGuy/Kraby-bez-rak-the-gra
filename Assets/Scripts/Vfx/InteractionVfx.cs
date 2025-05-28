using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Animator))]
public class InteractionVfx : MonoBehaviour
{
	private InteractableObject _interactable;
	private Animator _animator;
	private VisualEffect _vfx;

	[SerializeField, Min(0f)]
	private float _updateRate = 1f;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_vfx = this.TryGetInChildren<VisualEffect>();

		_interactable = this.TryGetInParent<InteractableObject>();
		_interactable.interacted += OnInteracted;
	}

	private void OnEnable()
	{
		StartCoroutine(CheckState());
	}

	private IEnumerator CheckState()
	{
		while (true)
		{
			yield return new WaitForSeconds(_updateRate);

			_animator.SetBool("hidden", !_interactable.IsInteractable(Player.instance));
		}
	}

	private void OnInteracted(InteractableObject interactableObject)
	{
		_animator.SetTrigger("interact");
		_vfx.SendEvent("Interact");
	}
}
