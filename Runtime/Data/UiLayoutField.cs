using UnityEngine;
using UnityEngine.UI;

using fwp.gamepad.layout;

public class UiLayoutField : MonoBehaviour
{
    Image image;
    TMPro.TextMeshProUGUI text;
    CanvasGroup group;

    void Start()
    {
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();

        group = gameObject.AddComponent<CanvasGroup>();
    }

    public void apply(LayoutField layout)
    {
        image.sprite = layout.getIcon();
        text.SetText(layout.getLabel());
    }

}
