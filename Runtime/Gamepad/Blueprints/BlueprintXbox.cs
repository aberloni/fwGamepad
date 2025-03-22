using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.blueprint
{
    using state;

    /// <summary>
    /// +dual stick mapping
    /// +triggers
    /// +dpad
    /// </summary>
    [System.Serializable]
    public class BlueprintXbox : BlueprintSnes
    {
        public ControllerJoystickState leftJoy = new ControllerJoystickState(InputJoystickSide.LEFT);
        public ControllerJoystickState rightJoy = new ControllerJoystickState(InputJoystickSide.RIGHT);

        //public Vector2 LeftRaw => leftJoy.joystick;
        //public Vector2 RightRaw => rightJoy.joystick;

        public ControllerTriggerState trigLeft = new ControllerTriggerState();
        public ControllerTriggerState trigRight = new ControllerTriggerState();

        public BlueprintXbox(InputSubsCallbacks subs = null) : base(subs)
        { }

        override protected ControllerJoystickState getJoystick(InputJoystickSide side)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT: return leftJoy;
                case InputJoystickSide.RIGHT: return rightJoy;
                default: return base.getJoystick(side);
            }
        }

        override protected ControllerTriggerState getTrigger(InputTriggers side)
        {
            switch (side)
            {
                case InputTriggers.LT: return trigLeft;
                case InputTriggers.RT: return trigRight;
            }
            return null;
        }

        /// <summary>
        /// triggers
        /// </summary>
        public void inject(InputTriggers side, float value)
        {
            var tar = getTrigger(side);
            if (tar.inject(value)) subs.onTriggerPerformed?.Invoke(side, value);
        }


        /// <summary>
        /// some button have timings
        /// </summary>
        override public void update(float dt)
        {
            base.update(dt);

            updateJoystick(InputJoystickSide.LEFT, dpadJoy, dt);
            updateJoystick(InputJoystickSide.RIGHT, rightJoy, dt);
        }

        /// <summary>
        /// snap = not keyboard
        /// </summary>
        override public void mimic(InputJoystickSide side, Vector2 value, bool snap)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT: leftJoy.injectIntention(value, snap); break;
                case InputJoystickSide.RIGHT: rightJoy.injectIntention(value, snap); break;
                default: base.mimic(side, value, snap); break;
            }
        }

        override public void mimicDirection(InputJoystickSide side, Vector2 dir)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT: leftJoy.injectDirection(dir); break;
                case InputJoystickSide.RIGHT: rightJoy.injectDirection(dir); break;
                default: base.mimicDirection(side, dir); break;
            }
        }

    }

}
