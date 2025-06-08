using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fwp.gamepad.blueprint
{
	using state;

	abstract public class Blueprint
	{
		public InputSubsCallbacks subs = null; // reactor

		public List<ControllerState> buffer = new();

		public Blueprint(InputSubsCallbacks callbacks = null)
		{
			subs = callbacks;
		}

		/// <summary>
		/// might wanna add diagonals ?
		/// </summary>
		abstract protected ControllerButtonState getDpad(InputDPad type);

		/// <summary>
		/// can add more buttons
		/// </summary>
		abstract protected ControllerButtonState getButton(InputActions type);

		abstract protected ControllerTriggerState getTrigger(InputTriggers type);

		protected void addBuffer(ControllerState state)
		{
			buffer.Add(state.Clone());

			//Debug.Log("+buffer " + buffer.Count);
		}

		public void inject(InputActions type, bool state)
		{
			var tar = getButton(type);
			if (tar == null)
			{
				Debug.LogWarning("blueprint " + GetType() + " missing button : " + type);
				return;
			}

			if (tar.inject(state))
			{
				log("button         " + type + "=" + state);
				subs.onButtonPerformed?.Invoke(type, state);

				// only press
				if (state) addBuffer(tar);
			}
		}

		virtual public void update(float dt)
		{ }

		protected void log(string content) => GamepadVerbosity.sLog(GetType() + " > " + content, this);
	}

}