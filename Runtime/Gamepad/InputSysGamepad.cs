using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace fwp.gamepad
{
    /// <summary>
    /// where the mapping with InputSystem is made
    /// 
    /// this is where InputSubs is sub to InputSystem
    /// any external object can use the ref of the subs to sub to it
    /// 
    /// this will sub callbacks to InputSystem
    /// and inject input into a "blueprint" controller
    /// that will solve input and trigger subs callbacks
    /// 
    /// mono to select type of controller
    /// and provide entry points for inputs
    /// 
    /// # unity.documentation
    /// 
    /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html
    /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Devices.html
    /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Layouts.html
    /// 
    /// </summary>
    public class InputSysGamepad : MonoBehaviour
    {
        /// <summary>
        /// first layer after InputSystem and lib
        /// transfert to blueprint to solve additionnal potential events
        /// down the line it's the blueprint that will trigger callbacks
        /// 
        /// sub to this object to get callbacks from blueprint solving
        /// TODO : allow RAW signal subing to go around blueprint
        /// </summary>
        public InputSubsCallbacks subs;

        public enum InputController
        {
            any,
            keyboard,
            mouse,
            gamepad_0,
            gamepad_1,
        }

        /// <summary>
        /// names setup in InputActionAsset mapping
        /// </summary>
        public enum MappingActions
        {
            joyLeft, joyRight,
            bumperL, bumperR,
            triggerL, triggerR,
            buttonSouth, buttonNorth, buttonWest, buttonEast,
            buttonStart, buttonBack,
            dpadNorth, dpadSouth, dpadWest, dpadEast
        }


        /// <summary>
        /// shortcut to call callbacks from 
        /// </summary>
        InputAction action(MappingActions act)
        {
            Debug.Assert(sysPlayerInput != null, "PlayerInput not fed");

            try
            {
                return sysPlayerInput.actions[act.ToString()];
            }
            catch
            {
                Debug.LogError("no action : " + act, this);
            }

            return null;
        }

        [Header("setup")]

        /// <summary>
        /// this controls what device to target
        /// </summary>
        public InputController controllerType;

        /// <summary>
        /// unity.device > handle of matching device in Unity's enviro
        /// </summary>
        public InputDevice sysDevice;

        /// <summary>
        /// unity.PlayerInput > link with unity InputSystem
        /// </summary>
        public PlayerInput sysPlayerInput;

        /// <summary>
        /// wrapper containing solved state of the controller after inputs
        /// last known state
        /// </summary>
        blueprint.BlueprintXbox controller;

        /// <summary>
        /// shk, to access internal controller state
        /// </summary>
        public blueprint.BlueprintXbox Controller => controller;

        private void Awake()
        {
            subs = new InputSubsCallbacks();

            log("created subs");

            if (sysPlayerInput == null)
            {
                sysPlayerInput = GetComponent<PlayerInput>();
            }

            Debug.Assert(sysPlayerInput != null, "need component PlayerInput");

            log(sysPlayerInput.name + " feed()");

            sysDevice = getDeviceIndex(controllerType);

            // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.1/api/UnityEngine.InputSystem.PlayerInputManager.html
            //var map = pi.actions.FindActionMap("mapping");

            if (sysDevice == null) Debug.LogWarning("controller <color=red>failed to bind</color> @" + controllerType);
            else
            {
                log("controls generated");

                controller = new blueprint.BlueprintXbox(subs);

                setupJoysticks();

                setupPad();
                setupBumpers();
                setupTriggers();

                setupDpad();

                //sysDevice.enabled = true;
            }
        }

        private void Update()
        {
            controller?.update(Time.deltaTime);
        }

        [ContextMenu("log devices")]
        void logDevices()
        {
            Debug.Log("devices x" + InputSystem.devices.Count);
            for (int i = 0; i < InputSystem.devices.Count; i++)
            {
                Debug.Log($"#{i} {InputSystem.devices[i]}");
            }
        }

        InputDevice getDeviceIndex(InputController device)
        {

            if (Gamepad.all.Count <= 0)
            {
                Debug.LogWarning(name + " no gamepad connected", this);
            }

            switch (device)
            {
                case InputController.gamepad_0:
                    if (Gamepad.all.Count > 0) return Gamepad.all[0];

                    device = InputController.keyboard;
                    Debug.LogWarning("no gamepad for #0 : <color=orange>using keyboard instead</color>");

                    break;
                case InputController.gamepad_1:
                    if (Gamepad.all.Count > 1) return Gamepad.all[1];
                    break;
            }

            if (device == InputController.keyboard)
            {
                return Keyboard.current;
            }

            Debug.LogWarning("could not solve device @ " + device, this);
            //Debug.LogWarning("gamepad x" + Gamepad.all.Count);
            logDevices();

            return null;
        }

        void setupBumpers()
        {
            // bumpers L/R

            action(MappingActions.bumperL).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.BL, true);
            };

            action(MappingActions.bumperL).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.BL, false);
            };

            action(MappingActions.bumperR).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.BR, true);
            };

            action(MappingActions.bumperR).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.BR, false);
            };

        }

        void setupTriggers()
        {

            // trigger L/R

            action(MappingActions.triggerL).performed += (InputAction.CallbackContext ctx) =>
            {
                float val = ctx.ReadValue<float>();
                controller.inject(InputTriggers.LT, val);
            };

            action(MappingActions.triggerL).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputTriggers.LT, 0f);
            };

            action(MappingActions.triggerR).performed += (InputAction.CallbackContext ctx) =>
            {
                float val = ctx.ReadValue<float>();
                controller.inject(InputTriggers.RT, val);
            };
            action(MappingActions.triggerR).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputTriggers.RT, 0f);
            };

        }

        void setupPad()
        {
            // pad buttons

            action(MappingActions.buttonStart).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.START, true);
            };

            action(MappingActions.buttonStart).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.START, false);
            };

            // le bouton du nord parmis les XYBA
            action(MappingActions.buttonNorth).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.ACT_NORTH, true);
            };

            action(MappingActions.buttonNorth).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.ACT_NORTH, false);
            };

            // le bouton du sud parmis les XYBA
            action(MappingActions.buttonSouth).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.ACT_SOUTH, true);
            };

            action(MappingActions.buttonSouth).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.ACT_SOUTH, false);
            };

            // le bouton de gauche parmis les XYBA
            action(MappingActions.buttonWest).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.ACT_WEST, true);
            };

            action(MappingActions.buttonWest).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.ACT_WEST, false);
            };

            // le bouton de droite parmis les XYBA
            action(MappingActions.buttonEast).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.ACT_EAST, true);
            };

            action(MappingActions.buttonEast).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputActions.ACT_EAST, false);
            };

        }

        void setupDpad()
        {
            action(MappingActions.dpadSouth).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputDPad.DPAD_SOUTH, true);
            };

            action(MappingActions.dpadSouth).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputDPad.DPAD_SOUTH, false);
            };

            action(MappingActions.dpadWest).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputDPad.DPAD_WEST, true);
            };

            action(MappingActions.dpadWest).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputDPad.DPAD_WEST, false);
            };

            action(MappingActions.dpadEast).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputDPad.DPAD_EAST, true);
            };
            action(MappingActions.dpadEast).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputDPad.DPAD_EAST, false);
            };

            action(MappingActions.dpadNorth).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputDPad.DPAD_NORTH, true);
            };
            action(MappingActions.dpadNorth).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputDPad.DPAD_NORTH, false);
            };

        }

        void setupJoysticks()
        {
            //left

            action(MappingActions.joyLeft).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputJoystickSide.LEFT, ctx.ReadValue<Vector2>(), !ctx.IsKeyboard());
            };

            action(MappingActions.joyLeft).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputJoystickSide.LEFT, Vector2.zero);
            };

            //right

            action(MappingActions.joyRight).performed += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputJoystickSide.RIGHT, ctx.ReadValue<Vector2>(), !ctx.IsKeyboard());
            };

            action(MappingActions.joyRight).canceled += (InputAction.CallbackContext ctx) =>
            {
                controller.inject(InputJoystickSide.RIGHT, Vector2.zero, !ctx.IsKeyboard());
            };

        }

        void log(string content) => GamepadVerbosity.sLog(GetType() + "&" + name + " >>> " + content, this);

    }



}
