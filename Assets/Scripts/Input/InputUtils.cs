using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public static class InputUtils
{
	/// <summary>
	/// Checks whether any key is down.
	/// </summary>
	public static bool AnyKeyDown(InputEventPtr eventPtr, InputDevice device)
	{
		if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
			return false;

		if (device is Keyboard || device is Gamepad || device is Pointer)
		{
			foreach (var control in device.allControls)
			{
				if (device is Keyboard keyboard && control == keyboard.escapeKey ||
					device is Gamepad gamepad && control == gamepad.startButton)
				{
					continue;
				}

				if (control is ButtonControl buttonControl)
				{
					if (buttonControl.ReadValueFromEvent(eventPtr) > 0.5f)
					{
						if (!(control.noisy || control.synthetic))
						{
							return true;
						}
					}
				}
			}
		}

		return false;
	}
}
