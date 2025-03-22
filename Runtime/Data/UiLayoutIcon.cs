using UnityEngine;

using UnityEngine.UI;

using fwp.gamepad.layout;

public class UiLayoutIcon : MonoBehaviour
{
    public Image image;

    virtual protected void Awake()
    {
        fetch();
        mimic(false);
    }

    virtual protected void fetch()
    {
        if (image == null) image = GetComponentInChildren<Image>();
    }

    public void mimic(bool press)
    {
        image.color = press ? Color.red : Color.white;
    }

    public void applyIcon(Sprite icon)
    {
        if (!Application.isPlaying) fetch();

        if (image != null) image.sprite = icon;
    }
}
