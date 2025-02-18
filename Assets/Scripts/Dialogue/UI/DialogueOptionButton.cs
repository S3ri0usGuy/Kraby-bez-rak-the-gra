using System;
using TMPro;
using UnityEngine;

/// <summary>
/// A component that represents a UI button for the dialogue option.
/// </summary>
public class DialogueOptionButton : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _numerLabel;
	[SerializeField]
	private TMP_Text _label;

	/// <summary>
	/// Gets/sets the index of the option.
	/// </summary>
	public int optionIndex { get; set; }

	/// <summary>
	/// An event that is triggered by clicking the button.
	/// </summary>
	public event Action<DialogueOptionButton> clicked;

	private void Start()
	{
		_numerLabel.text = (optionIndex + 1).ToString();
	}

	/// <summary>
	/// Sets the text of the button to a string.
	/// </summary>
	/// <param name="text">A string to set the text of the button.</param>
	public void SetText(string text)
	{
		_label.text = text;
	}

	/// <summary>
	/// A method that is called when the button is clicked.
	/// </summary>
	public void OnClick()
	{
		clicked?.Invoke(this);
	}
}
