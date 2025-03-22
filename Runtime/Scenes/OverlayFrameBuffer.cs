using UnityEngine;

using fwp.gamepad;

public class OverlayFrameBuffer : MonoBehaviour
{
    InputSysGamepad[] sysGamepads;

    public ViewFrameBuffer[] buffers;

    public bool dirty = false;

    private void Start()
    {
        sysGamepads = GameObject.FindObjectsOfType<InputSysGamepad>();
        Debug.Assert(sysGamepads != null, "no sys gamepad ?");
    }

    private void OnValidate()
    {
        if (Application.isPlaying) return;
        if(dirty) dirty = false;
    }

    private void Update()
    {
        if(dirty)
        {
            dirty = false;
            refresh();
        }
    }

    public void refresh()
    {
        Debug.Log("refresh : " + sysGamepads);

        if (sysGamepads == null)
            return;

        if(sysGamepads.Length <= 0)
        {
            Debug.LogWarning("no gamepads ?");
            return;
        }

        for (int i = 0; i < buffers.Length; i++)
        {
            if (i >= sysGamepads.Length) continue;

            buffers[i].refresh(sysGamepads[i]);
        }
    }
}
