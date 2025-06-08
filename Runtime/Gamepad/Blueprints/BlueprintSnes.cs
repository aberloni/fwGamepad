using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.blueprint
{
	using state;

	/// <summary>
	/// NES
	/// +shoulders
	/// +NS actions
	/// </summary>
	[System.Serializable]
	public class BlueprintSnes : BlueprintNes
	{
		public ControllerButtonState shoulderLeft = new ControllerButtonState(InputActions.BL);
		public ControllerButtonState shoulderRight = new ControllerButtonState(InputActions.BR);

		public ControllerButtonState pad_north = new ControllerButtonState(InputActions.ACT_NORTH);  // Y
		public ControllerButtonState pad_west = new ControllerButtonState(InputActions.ACT_WEST);   // X

		public BlueprintSnes(InputSubsCallbacks subs = null) : base(subs)
		{ }

		override protected ControllerButtonState getButton(InputActions type)
		{
			switch (type)
			{
				// X,Y
				case InputActions.ACT_NORTH: return pad_north;
				case InputActions.ACT_WEST: return pad_west;

				// L,R
				case InputActions.BL: return shoulderLeft;
				case InputActions.BR: return shoulderRight;
				default: return base.getButton(type);
			}
		}

	}
}
