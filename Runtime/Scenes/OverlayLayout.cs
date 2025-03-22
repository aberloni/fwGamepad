using UnityEngine;

namespace fwp.gamepad.layout
{

    public class OverlayLayout : MonoBehaviour
    {
        public DataGamepadLayout layout;

        public Transform pads;
        public Transform dpads;

        public bool apply = false;

        InputSysGamepad sysGamepad;

        private void Start()
        {
            sysGamepad = GameObject.FindObjectOfType<InputSysGamepad>();
            Debug.Assert(sysGamepad != null, "no sys gamepad ?");

            setupCallbacks();
        }

        void setupCallbacks()
        {
            var subs = sysGamepad.subs;

            subs.onJoystickPerformed += onJoystick;
            subs.onJoystickReleased += onJoystickRelease;
            subs.onJoystickDirection += onJoyDirection;
            subs.onJoystickPunchDirection += onJoyPunch;

            subs.onTriggerPerformed += onTrigger;
            subs.onButtonPerformed += onButton;
            subs.onDPadPerformed += onDPad;

            //Debug.Log("watcher:ON");
        }

        public void clearCallbacks()
        {
            var subs = sysGamepad.subs;

            subs.onJoystickDirection -= onJoyDirection;
            subs.onJoystickPerformed -= onJoystick;

            subs.onJoystickReleased -= onJoystickRelease;
            subs.onTriggerPerformed -= onTrigger;
            subs.onButtonPerformed -= onButton;
            subs.onDPadPerformed -= onDPad;
        }

        void onJoystickRelease(InputJoystickSide side)
        {
            onJoystick(side, Vector2.zero);
        }

        void onJoystick(InputJoystickSide side, Vector2 value)
        {
            var f = getField<InputJoystickSide>(InputType.JOYS, side);
            Debug.Log(f);
        }

        void onJoyDirection(InputJoystickSide side, Vector2 value)
        {
            
        }

        void onJoyPunch(InputJoystickSide side, Vector2 value)
        {
            
        }

        void onTrigger(InputTriggers side, float value)
        {
            
        }

        private void onButton(InputButtons type, bool status)
        {
            
        }

        private void onDPad(InputDPad type, bool status)
        {
            
        }

        private void OnValidate()
        {
            applyLayout(layout);

            apply = false;
        }

        /// <summary>
        /// 
        /// </summary>
        UiLayoutField getField<TType>(InputType cat, TType input)
        {
            var fields = GetComponentsInChildren<UiLayoutField>();
            Debug.Log(input + " x" + fields.Length);

            foreach(var f in fields)
            {
                if(f.inputType == cat)
                {
                    if(f.name.EndsWith(input.ToString().ToLower()))
                    {
                        return f;
                    }
                }
            }
            return null;
        }

        void applyLayout(DataGamepadLayout layout)
        {
            this.layout = layout;
            if (this.layout == null)
                return;

            Debug.Log("oly.apply "+layout.name, layout);

            if (pads != null)
            {
                foreach(var elmt in layout.getActions()) getField<InputButtons>(InputType.JOYS, elmt.input);
            }

            if(dpads != null)
            {
                foreach (var elmt in layout.getDpads()) getField<InputDPad>(InputType.DPAD, elmt.input);
            }
        }

        void applyField(Transform pivot, LayoutInputAction settings)
        {
            if (pivot == null) return;
            pivot.Find("icon").GetComponent<UnityEngine.UI.Image>().sprite = settings.getIcon();
            pivot.Find("label").GetComponent<TMPro.TextMeshProUGUI>().SetText(settings.getLabel());
        }
    }

}