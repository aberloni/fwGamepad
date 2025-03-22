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
        }

        void onJoystickRelease(InputJoystickSide side)
        {
            onJoystick(side, Vector2.zero);
        }

        void onJoystick(InputJoystickSide side, Vector2 value)
        {
            var f = getField<InputJoystickSide>(InputType.JOYS, side);
            if (f is UiLayoutJoystick j) j.mimicRaw(value);
        }

        void onJoyDirection(InputJoystickSide side, Vector2 value)
        {
            var f = getField<InputJoystickSide>(InputType.JOYS, side);
            if (f is UiLayoutJoystick j) j.mimicDirection(value);
        }

        void onJoyPunch(InputJoystickSide side, Vector2 value)
        {
            var f = getField<InputJoystickSide>(InputType.JOYS, side);
            if (f is UiLayoutJoystick j) j.mimicPunch(value);
        }

        void onTrigger(InputTriggers side, float value)
        {

        }

        private void onButton(InputActions type, bool status)
        {
            var f = getField<InputActions>(InputType.ACTIONS, type);
            if (f is UiLayoutField lyr) lyr.mimic(status);
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
            //Debug.Log(input + " x" + fields.Length);

            foreach (var f in fields)
            {
                if (f.inputType == cat)
                {
                    if (f.name.EndsWith(input.ToString().ToLower()))
                    {
                        return f;
                    }
                }
            }

            Debug.LogWarning("no " + cat + " & " + input);

            return null;
        }

        void applyLayout(DataGamepadLayout layout)
        {
            this.layout = layout;
            if (this.layout == null)
                return;

            Debug.Log("oly.apply " + layout.name, layout);

            foreach (var elmt in layout.getJoysticks()) 
                getField<InputJoystickSide>(InputType.JOYS, elmt.input).setLayout(elmt);
            foreach (var elmt in layout.getActions()) 
                getField<InputActions>(InputType.ACTIONS, elmt.input).setLayout(elmt);
        }

        void applyField(Transform pivot, LayoutInputAction settings)
        {
            if (pivot == null) return;
            pivot.Find("icon").GetComponent<UnityEngine.UI.Image>().sprite = settings.getIcon();
            pivot.Find("label").GetComponent<TMPro.TextMeshProUGUI>().SetText(settings.getLabel());
        }
    }

}