using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.blueprint
{
    using state;

    /// <summary>
    /// blueprint contains the state of the controller
    /// funnel all events and solve various substate/event linked to keypressing
    /// 
    /// NES
    ///     DPAD
    ///     ACTIONS EW
    ///     START/SELECT
    /// </summary>
    [System.Serializable]
    public class BlueprintNes : Blueprint
    {

        /// <summary>
        /// dpads are buttons and not joystick
        /// because on some controller you can press 2+ at the same time
        /// </summary>
        public ControllerButtonState dpad_north = new ControllerButtonState(InputActions.ACT_DNORTH);
        public ControllerButtonState dpad_south = new ControllerButtonState(InputActions.ACT_DSOUTH);
        public ControllerButtonState dpad_east = new ControllerButtonState(InputActions.ACT_DEAST);
        public ControllerButtonState dpad_west = new ControllerButtonState(InputActions.ACT_DWEST);

        public ControllerJoystickState dpadJoy = new ControllerJoystickState(InputJoystickSide.DPAD);

        /// <summary>
        /// to vector2D
        /// </summary>
        public Vector2 dpadVec => new Vector2(
            dpad_west.state ? -1 : dpad_east.state ? 1 : 0f,
            dpad_south.state ? -1 : dpad_north.state ? 1 : 0f);

        public ControllerButtonState start = new ControllerButtonState(InputActions.START);
        public ControllerButtonState back = new ControllerButtonState(InputActions.RETURN);

        public ControllerButtonState pad_south = new ControllerButtonState(InputActions.ACT_SOUTH);  // A
        public ControllerButtonState pad_east = new ControllerButtonState(InputActions.ACT_EAST);   // B

        public BlueprintNes(InputSubsCallbacks subs = null) : base(subs)
        { }


        virtual protected ControllerJoystickState getJoystick(InputJoystickSide side)
        {
            switch (side)
            {
                case InputJoystickSide.DPAD: return dpadJoy;
            }
            return null;
        }

        protected override ControllerTriggerState getTrigger(InputTriggers type)
        {
            return null;
        }

        /// <summary>
        /// might wanna add diagonals ?
        /// </summary>
        override protected ControllerButtonState getDpad(InputDPad type)
        {
            switch (type)
            {
                case InputDPad.DPAD_NORTH: return dpad_north;
                case InputDPad.DPAD_EAST: return dpad_east;
                case InputDPad.DPAD_SOUTH: return dpad_south;
                case InputDPad.DPAD_WEST: return dpad_west;
            }
            return null;
        }

        /// <summary>
        /// declare all buttons of this layout here
        /// </summary>
        override protected ControllerButtonState getButton(InputActions type)
        {
            switch (type)
            {
                // pad A,B
                case InputActions.ACT_SOUTH: return pad_south;
                case InputActions.ACT_EAST: return pad_east;
				
                // dpad
				case InputActions.ACT_DNORTH: return dpad_north;
                case InputActions.ACT_DEAST: return dpad_east;
                case InputActions.ACT_DSOUTH: return dpad_south;
                case InputActions.ACT_DWEST: return dpad_west;

                // others
                case InputActions.START: return start;
				case InputActions.RETURN: return back;
			}
            return null;
        }


        public void inject(InputDPad dpad, bool status)
        {
            inject(getDpad(dpad).input, status);
            inject(InputJoystickSide.DPAD, dpadVec);
        }

        public void inject(InputJoystickSide side, Vector2 intention, bool snap = true)
        {
            var tar = getJoystick(side);

            log("inject " + side + " " + intention + " snap?" + snap);

            if (tar.injectIntention(intention, snap))
            {
                log("intention : " + intention + " & " + tar.joystick);
                //subs.onJoystickPerformed?.Invoke(side, tar.joystick);
                if (snap) solveCallbacks(side, tar);
            }
        }

        /// <summary>
        /// snap = not keyboard
        /// </summary>
        virtual public void mimic(InputJoystickSide side, Vector2 value, bool snap)
        {
            switch (side)
            {
                case InputJoystickSide.DPAD: dpadJoy.injectIntention(value, snap); break;
            }
        }
        virtual public void mimicDirection(InputJoystickSide side, Vector2 dir)
        {
            switch (side)
            {
                case InputJoystickSide.DPAD: dpadJoy.injectDirection(dir); break;
            }
        }







        /// <summary>
        /// some button have timings
        /// </summary>
        override public void update(float dt)
        {
            base.update(dt);

            updateJoystick(InputJoystickSide.DPAD, dpadJoy, dt);
        }

        protected void updateJoystick(InputJoystickSide side, ControllerJoystickState j, float dt)
        {
            j.updateTimer(dt);

            //Debug.Log(side + " & " + j.joystick);

            if (j.updateIntention(dt))
            {
                solveCallbacks(side, j);
            }

        }


        void solveCallbacks(InputJoystickSide side, ControllerJoystickState j)
        {
            if (subs == null)
                return;

            if (j.joystick.sqrMagnitude == 0f)
            {
                subs?.onJoystickReleased(side);
                log("punched ? " + j.Punch + " @ " + j.PunchTimer);
                if (j.Punch)
                {
                    log("punched !");
                    subs.onJoystickPunchDirection?.Invoke(side, j.joystickDirection);
                }
            }
            else
            {
                // positive magnitude
                subs?.onJoystickPerformed(side, j.joystick);
            }

            if (j.injectDirection(j.joystick))
            {
                log("direction : " + j.joystickDirection);
                subs.onJoystickDirection?.Invoke(side, j.joystickDirection);
            }
        }

    }

}
