using UnityEngine;

namespace fwp.gamepad.layout
{
    /// <summary>
    /// any controller with 2x sticks
    /// directional pad
    /// action buttons
    /// </summary>

    [CreateAssetMenu(menuName = "gamepad/layout/xbox", order = 100)]
    public class DataLayoutXbox : DataGamepadLayout,
        LayoutJoystickDual, LayoutJoystickActionsNESW,
        LayoutJoystickActionsMenus, LayoutJoystickDirectionalPad
    {
        public DataLayoutXbox()
        {
            addSticks<InputJoystickSide>();
            addPads<InputButtons>();
            addDpads<InputDPad>();
        }

        public LayoutInputJoystick getLeftStick() => get(InputJoystickSide.LEFT);
        public LayoutInputJoystick getRightStick() => get(InputJoystickSide.RIGHT);

        public LayoutInputAction getActionNorth() => get(InputButtons.PAD_NORTH);
        public LayoutInputAction getActionEast() => get(InputButtons.PAD_EAST);
        public LayoutInputAction getActionSouth() => get(InputButtons.PAD_SOUTH);
        public LayoutInputAction getActionWest() => get(InputButtons.PAD_WEST);

        public LayoutInputAction getActionStart() => get(InputButtons.START);
        public LayoutInputAction getActionSelect() => get(InputButtons.RETURN);

        public LayoutInputDpad getDpadNorth() => get(InputDPad.DPAD_NORTH);
        public LayoutInputDpad getDpadEast() => get(InputDPad.DPAD_EAST);
        public LayoutInputDpad getDpadSouth() => get(InputDPad.DPAD_SOUTH);
        public LayoutInputDpad getDpadWest() => get(InputDPad.DPAD_WEST);

        public override LayoutInputJoystick[] getJoysticks() =>
            new[] { getLeftStick(), getRightStick() };

        public override LayoutInputDpad[] getDpads() =>
            new[] { getDpadNorth(), getDpadEast(), getDpadSouth(), getDpadWest() };

        public override LayoutInputAction[] getActions() =>
            new[] { getActionNorth(), getActionEast(), getActionSouth(), getActionWest() };
    }
}
