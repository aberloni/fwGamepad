using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fwp.gamepad.layout
{
    using fwp.gamepad.state;

    /// <summary>
    /// image & labels assigned to a gamepad
    /// for each button/input/interactions
    /// </summary>
    abstract public class DataGamepadLayout : ScriptableObject
    {
        [Header("output (readonly)")]

        [SerializeField]
        List<LayoutInputJoystick> joysticks = new();

        /// <summary>
        /// anything press/release
        /// </summary>
        [SerializeField]
        List<LayoutInputAction> actions = new();

        /// <summary>
        /// dpad
        /// </summary>
        [SerializeField]
        List<LayoutInputAction> dpads = new();

        public LayoutInputJoystick get(InputJoystickSide input) => joysticks[((int)input) - 1];
        public LayoutInputAction get(InputDPad input) => dpads[((int)input) - 1];
        public LayoutInputAction get(InputActions input) => actions[((int)input) - 1];

        abstract public LayoutInputJoystick[] getJoysticks();
        abstract public LayoutInputAction[] getDpads();
        abstract public LayoutInputAction[] getActions();

        virtual public LayoutField getStateField(ControllerState state)
        {
            if (state is ControllerJoystickState j)
            {
                return get(j.input);
            }
            if(state is ControllerButtonState b)
            {
                return get(b.input);
            }
            return null;
        }

        public void addSticks<T>() where T : System.Enum
        {
            System.Type t = typeof(T);
            Debug.Assert(t.IsEnum, "not enum ?");
            foreach(T item in System.Enum.GetValues(t)) addJoystick((InputJoystickSide)(object)item);
        }

        public void addActions<T>() where T : System.Enum
        {
            System.Type t = typeof(T);
            Debug.Assert(t.IsEnum, "not enum ?");
            foreach (T item in System.Enum.GetValues(t)) addAction((InputActions)(object)item);
        }

        public void addDpads<T>() where T : System.Enum
        {
            System.Type t = typeof(T);
            Debug.Assert(t.IsEnum, "not enum ?");
            foreach (T item in System.Enum.GetValues(t)) addDpad((InputDPad)(object)item);
        }

        public void addJoystick(InputJoystickSide val)
        {
            if (val == InputJoystickSide.NONE) return;
            joysticks.Add(new LayoutInputJoystick(val));
        }

        public void addAction(InputActions val)
        {
            if (val == InputActions.NONE) return;
            actions.Add(new LayoutInputAction(val));
        }

        public void addDpad(InputDPad val)
        {
            if (val == InputDPad.NONE) return;
            switch (val)
            {
                case InputDPad.DPAD_NORTH:dpads.Add(new LayoutInputAction(InputActions.ACT_DNORTH));break;
                case InputDPad.DPAD_SOUTH: dpads.Add(new LayoutInputAction(InputActions.ACT_DSOUTH)); break;
                case InputDPad.DPAD_EAST: dpads.Add(new LayoutInputAction(InputActions.ACT_DEAST)); break;
                case InputDPad.DPAD_WEST: dpads.Add(new LayoutInputAction(InputActions.ACT_DWEST)); break;
            }
        }
    }

    [System.Serializable]
    public class LayoutInputJoystick : LayoutInput<InputJoystickSide>
    {
        public LayoutInputJoystick(InputJoystickSide input) : base(input)
        {
        }
    }

    [System.Serializable]
    public class LayoutInputAction : LayoutInput<InputActions>
    {
        public LayoutInputAction(InputActions input) : base(input)
        {
        }
    }

    /*
    [System.Serializable]
    public class LayoutInputDpad : LayoutInput<InputDPad>
    {
        public LayoutInputDpad(InputDPad input) : base(input)
        {
        }
    }
    */

    [System.Serializable]
    abstract public class LayoutInput<TInput> : LayoutField
    {
        public TInput input;

        public LayoutInput(TInput input)
        {
            this.input = input;
            label = input.ToString();
        }

    }

    [System.Serializable]
    abstract public class LayoutField : iLayoutIcon, iLayoutLabel
    {
        [SerializeField]
        protected string label;

        [SerializeField]
        protected Sprite icon;
        
        public Sprite getIcon() => icon;

        public string getLabel() => label;
    }

    [System.Serializable]
    public struct LayoutFieldParams
    {
        public Sprite icon;
        public string label;
    }

    public interface iLayoutLabel
    {
        public string getLabel();
    }

    public interface iLayoutIcon
    {
        public Sprite getIcon();
    }

}
