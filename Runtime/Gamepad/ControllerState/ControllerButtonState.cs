using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.state
{
    /// <summary>
    /// a simple button pressed & released
    /// </summary>
    [System.Serializable]
    public class ControllerButtonState : ControllerState
    {
        public InputActions input;

        int frame;
        public bool state;
        public bool pressed => state & frame == Time.frameCount;
        public bool released => !state & frame == Time.frameCount;

        public ControllerButtonState(InputActions input) :base(InputType.ACTIONS)
        {
            this.input = input;
        }

        /// <summary>
        /// true : state changed
        /// </summary>
        public bool inject(bool newState)
        {
            frame = Time.frameCount;
            if(state != newState)
            {
                state = newState;
                return true;
            }
            return false;
        }
    }
}
