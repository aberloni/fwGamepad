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
            addActions<InputActions>();
            //addDpads<InputDPad>();
        }

        public LayoutInputJoystick getLeftStick() => get(InputJoystickSide.LEFT);
        public LayoutInputJoystick getRightStick() => get(InputJoystickSide.RIGHT);
        public LayoutInputJoystick getDpad() => get(InputJoystickSide.DPAD);

        public LayoutInputAction getActionNorth() => get(InputActions.ACT_NORTH);
        public LayoutInputAction getActionEast() => get(InputActions.ACT_EAST);
        public LayoutInputAction getActionSouth() => get(InputActions.ACT_SOUTH);
        public LayoutInputAction getActionWest() => get(InputActions.ACT_WEST);

        public LayoutInputAction getActionStart() => get(InputActions.START);
        public LayoutInputAction getActionSelect() => get(InputActions.RETURN);

        public LayoutInputAction getDpadNorth() => get(InputActions.ACT_DNORTH);
        public LayoutInputAction getDpadEast() => get(InputActions.ACT_DEAST);
        public LayoutInputAction getDpadSouth() => get(InputActions.ACT_DSOUTH);
        public LayoutInputAction getDpadWest() => get(InputActions.ACT_DWEST);
        
        public override LayoutInputJoystick[] getJoysticks() =>
            new[] { getLeftStick(), getRightStick() };

        public override LayoutInputAction[] getDpads() =>
            new[] { getDpadNorth(), getDpadEast(), getDpadSouth(), getDpadWest() };

        public override LayoutInputAction[] getActions() =>
            new[] { getActionNorth(), getActionEast(), getActionSouth(), getActionWest() };
    }
}
