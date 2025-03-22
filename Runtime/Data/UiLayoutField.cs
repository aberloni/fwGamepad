using UnityEngine;
using UnityEngine.UI;

using fwp.gamepad;
using fwp.gamepad.state;
using fwp.gamepad.layout;

/// <summary>
/// view
/// </summary>
public class UiLayoutField : UiLayoutIcon
{
    public InputType inputType;

    public TMPro.TextMeshProUGUI text;

    override protected void fetch()
    {
        base.fetch();
        if (text == null) text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void setLayout(LayoutField field)
    {
        if (!Application.isPlaying) fetch();
        
        applyIcon(field.getIcon());

        if (text != null) text.SetText(field.getLabel());
    }

}
