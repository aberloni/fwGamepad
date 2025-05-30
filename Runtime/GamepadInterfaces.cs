﻿using UnityEngine;

namespace fwp.gamepad
{
	/// <summary>
	/// interface pour les candidates de la selection
	/// </summary>
	public interface ISelectable
	{
		// event when object is target of selection
		public void onSelected();

		// event when object just lost selection
		public void onUnselected();

		// control over if object is candidate for selection
		public bool isSelectable();

	}

	/// <summary>
	/// est-ce que si l'objet est select
	/// les autres objets de la queue vont aussi prendre les inputs
	/// </summary>
	public interface ISelectableAbsorb : ISelectable
	{
		/// <summary>
		/// this element needs to lock inputs for other (if multi selection)
		/// </summary>
		public bool isAbsorb();
	}

	/// <summary>
	/// toutes les interfaces suivantes lie a l'utilisation des boutons de la manette
	/// sont utilises par les Kappa direct
	/// </summary>

	public interface ISelectableDpad
	{
		public bool onDPad(InputDPad evt, bool status);
	}

    public interface ISelectableJoyDirection : ISelectableInput
    {
        public void onJoyLeftDir(Vector2 value);
        public void onJoyRightDir(Vector2 value);
    }

	/// <summary>
	/// when joystick point a direction and cancel quickly
	/// </summary>
	public interface ISelectableJoyPunch : ISelectableInput
	{
		public void onJoyLeftPunch(Vector2 value);
		public void onJoyRightPunch(Vector2 value);
	}

	public interface ISelectableJoy : ISelectableInput
    {
		public void onJoy(InputJoystickSide side, Vector2 value);
		public void onJoyDirection(InputJoystickSide side, Vector2 value);
		public void onJoyPunch(InputJoystickSide side, Vector2 value);
	}

	/// <summary>
	/// raw signal from both joysticks
	/// </summary>
	public interface ISelectableJoys : ISelectableInput
    {
		public void onJoyLeft(Vector2 value);
		public void onJoyRight(Vector2 value);
	}

	public interface ISelectableTrigger
	{
		public void onTrigLeft(float value);
		public void onTrigRight(float value);
	}

	public interface ISelectableButton
	{
		/// <summary>
		/// Appel� lorsqu'un bouton est enfonc� ou rel�ch�.
		/// Retourner true stoppe la propagation de l'�v�nement.
		/// </summary>
		/// <param name="type">Quel bouton a �t� utilis�</param>
		/// <param name="status">true = button down, false = button up</param>
		/// <returns>Stop event propagation</returns>
		public bool onButton(InputActions type, bool status);
	}

	/// <summary>
	/// SHORTCUTS
	/// shortcut for multiple interfaces
	/// </summary>
	public interface ISelectableController : ISelectableJoy, ISelectableDpad, ISelectableTrigger
	{ }

	public interface ISelectableInput
	{ }
}
