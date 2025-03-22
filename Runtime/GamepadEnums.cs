namespace fwp.gamepad
{
    [System.Serializable]
    public enum InputType
    {
        JOYS,           // [X,Y]
        ACTIONS,        // [press,release]
        TRIGGER,        // [0,1]
    }

    /// <summary>
    /// anything press/release
    /// </summary>
    [System.Serializable]
    public enum InputActions
    {
        NONE,
        ACT_NORTH, ACT_EAST, ACT_SOUTH, ACT_WEST, // actions
        ACT_DNORTH, ACT_DEAST, ACT_DSOUTH, ACT_DWEST, // dpad
        START, RETURN,
        BL, BR // bumper aka shoulder (top)
    }

    /// <summary>
    /// NESW must match PadSectionDirection
    /// </summary>
    [System.Serializable]
    public enum InputDPad
    {
        NONE,
        DPAD_NORTH, DPAD_EAST, DPAD_SOUTH, DPAD_WEST,
    }

    /// <summary>
    /// [0,1]
    /// </summary>
    [System.Serializable]
    public enum InputTriggers
    {
        NONE,
        LT, RT,
    }

    [System.Serializable]
    public enum InputJoystickSide
    {
        NONE,
        LEFT, RIGHT, DPAD
    }

}
