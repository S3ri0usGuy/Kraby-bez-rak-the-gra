using System;
using UnityEngine;

/// <summary>
/// A set of parameters that defines how the dialogue answering behaves.
/// </summary>
[Serializable]
public class DialogueAnswerParams
{
	[SerializeField]
	private DialogueAnswerType _answerType = DialogueAnswerType.Default;
	[SerializeField, Min(0f)]
	[Tooltip("A time that is given to the player to answer. " +
		"Has no effect if the Answer Type is set to \"Default\".")]
	private float _timeToAnswer = 5f;

	/// <summary>
	/// Gets a type of the answer behaviour.
	/// </summary>
	public DialogueAnswerType answerType => _answerType;
	/// <summary>
	/// Gets a time that is given to the player to answer.
	/// Has no effect if the <see cref="answerType" /> is set to 
	/// <see cref="DialogueAnswerType.Default"/>.
	/// </summary>
	public float timeToAnswer => _timeToAnswer;
}
