using UnityEngine;

/// <summary>
/// A component that allows to complete or fail a the subquest. Can be
/// used by Unity events.
/// </summary>
public class SubquestAction : MonoBehaviour
{
	public enum SubquestActionType
	{
		Reveal,
		Complete,
		Fail
	}

	[SerializeField]
	private bool _performOnEnable = false;
	[SerializeField]
	[Tooltip("A quest that is the receiver of this action.")]
	private Subquest _subquest;
	[SerializeField]
	[Tooltip("An action to perform.")]
	private SubquestActionType _action;

	private void OnEnable()
	{
		if (_performOnEnable)
		{
			Perform();
		}
	}

	public void Perform()
	{
		QuestSystem questSystem = QuestSystem.instance;
		switch (_action)
		{
			case SubquestActionType.Reveal:
				if (questSystem.GetSubquestState(_subquest) == SubquestState.None)
				{
					questSystem.SetSubquestState(_subquest, SubquestState.Active);
				}
				break;

			case SubquestActionType.Complete:
				questSystem.SetSubquestState(_subquest, SubquestState.Completed);
				break;

			case SubquestActionType.Fail:
				questSystem.SetSubquestState(_subquest, SubquestState.Failed);
				break;
		}
	}
}
