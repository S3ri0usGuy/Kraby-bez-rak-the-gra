using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// A component that allows the player to interact with objects
/// that inherit the <see cref="Interactable" /> class.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
	public const float refreshRate = 0.5f;

	private Player _player;
	private InputActions.PlayerActions _actions;
	private Interactable _currentInteractable;

	[SerializeField]
	private TMP_Text _label;
	[SerializeField]
	private string _labelFormat = "{0} - {1}";
	[SerializeField]
	private LayerMask _layerMask;

	private void Awake()
	{
		_player = this.TryGetInParent<Player>();
	}

	private void OnEnable()
	{
		_actions = InputActionsProvider.RetrieveFor(this).player;

		_actions.interact.performed += OnInteractPressed;
		StartCoroutine(DelayedUpdate());

		SetCurrentInteractable(null);
	}

	private void OnDisable()
	{
		_actions.interact.performed -= OnInteractPressed;
	}

	private void Update()
	{
		if ((_currentInteractable && !_currentInteractable.IsInteractable(_player)) ||
			_player.movement.isFreezed)
		{
			SetCurrentInteractable(null);
		}
	}

	private void RefreshCurrentInteractable()
	{
#pragma warning disable UNT0028 // Use non-allocating physics APIs
		var colliders = Physics.OverlapSphere(
			transform.position + Vector3.up,
			_player.movement.controller.radius,
			_layerMask, QueryTriggerInteraction.Collide);
#pragma warning restore UNT0028 // Use non-allocating physics APIs

		List<Interactable> interactables = new();
		foreach (var collider in colliders)
		{
			if (collider.TryGetComponent<Interactable>(out var interactable))
			{
				interactables.Add(interactable);
			}
		}

		if (_currentInteractable)
		{
			if (!interactables.Contains(_currentInteractable))
			{
				SetCurrentInteractable(null);
			}
			else
			{
				return;
			}
		}
		var newInteractable = interactables
			.FirstOrDefault(x => x.IsInteractable(_player));
		if (newInteractable)
		{
			SetCurrentInteractable(newInteractable);
		}
	}

	private IEnumerator DelayedUpdate()
	{
		while (true)
		{
			yield return new WaitForSecondsRealtime(refreshRate);
			UpdateTextLabel();

			if (!_player.movement.isFreezed)
				RefreshCurrentInteractable();
		}
	}

	private void OnInteractPressed(InputAction.CallbackContext context)
	{
		if (_currentInteractable)
		{
			bool result = _currentInteractable.Interact(_player);
			if (result) SetCurrentInteractable(null);
		}
	}

	private void UpdateTextLabel()
	{
		if (_currentInteractable)
		{
			var operation = _currentInteractable.actionName.GetLocalizedStringAsync();
			operation.Completed += OnStringLoaded;
		}
	}

	private void OnStringLoaded(AsyncOperationHandle<string> asyncOperationHandle)
	{
		string key = _actions.interact.GetBindingDisplayString();
		_label.text = string.Format(_labelFormat, key, asyncOperationHandle.Result);
	}

	private void SetCurrentInteractable(Interactable interactable)
	{
		if (!interactable)
		{
			_currentInteractable = null;
			_label.enabled = false;

			return;
		}

		_currentInteractable = interactable;

		_label.enabled = true;
		UpdateTextLabel();
	}
}
