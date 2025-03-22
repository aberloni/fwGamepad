using UnityEngine;

namespace fwp.gamepad
{
    using fwp.gamepad.state;

    [CreateAssetMenu(menuName = "gamepad/buffer icons", order = 100)]
    public class DataBufferIcons : ScriptableObject
    {
        [System.Serializable]
        public struct BufferAction
        {
            public InputActions input;
            public Sprite icon;
        }

        [System.Serializable]
        public struct BufferDirection
        {
            public InputJoystickSide joystick;
            public Sprite icon;
            public Vector2 direction;
        }

        public BufferAction[] actions;
        public BufferDirection[] directions;

        public Sprite getIcon(ControllerState state)
        {
            if(state is ControllerJoystickState j)
            {
                return getIconDirection(j.input, j.joystickDirection);
            }
            if(state is ControllerButtonState b)
            {
                return getIconAction(b.input);
            }
            return null;
        }

        public Sprite getIconAction(InputActions input)
        {
            foreach(var a in actions)
            {
                if (a.input == input) return a.icon;
            }
            return null;
        }

        public Sprite getIconDirection(InputJoystickSide side, Vector2 dir)
        {
            foreach(var d in directions)
            {
                if (d.joystick != side) continue;
                if (Mathf.Sign(d.direction.x) == Mathf.Sign(dir.x))
                {
                    if (Mathf.Sign(d.direction.y) == Mathf.Sign(dir.y))
                        return d.icon;
                }
            }
            return null;
        }
    }

}