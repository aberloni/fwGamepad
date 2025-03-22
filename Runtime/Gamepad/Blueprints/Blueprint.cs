using UnityEngine;

namespace fwp.gamepad.blueprint
{
    using state;

    abstract public class Blueprint
    {
        public InputSubsCallbacks subs = null; // reactor

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
        abstract protected ControllerButtonState getButton(InputButtons type);

        public void inject(InputDPad type, bool state)
        {
            var tar = getDpad(type);
            if (tar.inject(state))
            {
                log("dpad       " + type + "=" + state);
                subs.onDPadPerformed?.Invoke(type, state);
            }
        }

        public void inject(InputButtons type, bool state)
        {
            var tar = getButton(type);
            if (tar.inject(state))
            {
                log("button         " + type + "=" + state);
                subs.onButtonPerformed?.Invoke(type, state);
            }
        }

        public void mimic(InputButtons type, bool state)
        {
            getButton(type).state = state;
        }

        public void mimic(InputDPad type, bool state)
        {
            getDpad(type).state = state;
        }

        virtual public void update(float dt)
        { }

        protected void log(string content) => GamepadVerbosity.sLog(GetType() + " > " + content, this);
    }

}