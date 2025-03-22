using UnityEngine;

using fwp.gamepad;
using fwp.gamepad.layout;

public class ViewFrameBuffer : MonoBehaviour
{
    public DataBufferIcons bufferIcons;

    GameObject slotRef; // first present in scene, to clone
    Transform pivot;

    public bool dirty = false;

    private void Start()
    {
        pivot = transform.Find("buffer");
        slotRef = pivot.GetChild(0).gameObject;
        slotRef.name = "slot-ref";

        wipe();
    }

    public void refresh(fwp.gamepad.InputSysGamepad gamepad)
    {
        var c = gamepad.Controller;
        if (c == null)
        {
            Debug.LogWarning("no controller ?");
            return;
        }

        wipe();

        Debug.Log("refresh buffer : " + name + " x" + c.buffer.Count, this);

        for (int i = 0; i < c.buffer.Count; i++)
        {
            GameObject slot = slotRef;
            if (i > 0) slot = GameObject.Instantiate(slotRef, pivot);

            var uiLayout = slot.GetComponent<UiLayoutIcon>();
            var icon = bufferIcons.getIcon(c.buffer[i]);
            Debug.Log(c.buffer[i].inputCateogry + " => " + icon);
            uiLayout.applyIcon(icon);
        }

    }

    void wipe()
    {
        while (pivot.childCount > 1)
        {
            Transform child = pivot.GetChild(pivot.childCount - 1);
            child.SetParent(null);
            GameObject.Destroy(child.gameObject);
        }
    }

}
