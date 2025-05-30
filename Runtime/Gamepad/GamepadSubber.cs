﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace fwp.gamepad
{

    /// <summary>
    /// this is a helper
    /// a wrapper around the InputSubs logic
    /// provide methods to subs to InputSubs
    /// </summary>
    public class GamepadSubber
    {
        InputSubsCallbacks controllerSubs; // this controller subs

        public GamepadSubber(GamepadWatcher controller)
        {
            controllerSubs = controller.playerInputSys.subs;

            Debug.Assert(controllerSubs != null, "no sub given ?");

        }

        public bool isReady() => controllerSubs != null;

        public void subTriggers(bool sub, Action<InputTriggers, float> performed)
        {
            if (sub)
            {
                if (performed != null) controllerSubs.onTriggerPerformed += performed;
            }
            else
            {
                if (performed != null) controllerSubs.onTriggerPerformed -= performed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void subButtons(bool sub, Action<InputActions, bool> performed)
        {
            if (sub)
            {
                if (performed != null) controllerSubs.onButtonPerformed += performed;
            }
            else
            {
                if (performed != null) controllerSubs.onButtonPerformed -= performed;
            }
        }

        /// <summary>
        /// pour les kappa de move
        /// performed = EACH FRAME until neutral
        /// release = when stick go back to neutral
        /// </summary>
        public void subJoysticks(bool sub,
            Action<InputJoystickSide, Vector2> performed,
            Action<InputJoystickSide> release = null)
        {
            if (sub)
            {
                if (performed != null) controllerSubs.onJoystickPerformed += performed;
                if (release != null) controllerSubs.onJoystickReleased += release;
            }
            else
            {
                if (performed != null) controllerSubs.onJoystickPerformed -= performed;
                if (release != null) controllerSubs.onJoystickReleased -= release;
            }

        }
    }

}