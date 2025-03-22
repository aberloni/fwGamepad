using UnityEngine;

public class UiLayoutJoystick : UiLayoutField
{
    public UnityEngine.UI.Image icon_press;
    public RectTransform vector_float_handle;
    public RectTransform vector_ints;
    public RectTransform vector_punchs;

    protected override void Awake()
    {
        base.Awake();

        mimicPunch(Vector2.zero);
        mimicDirection(Vector2.zero);
        mimicRaw(Vector2.zero);

        icon_press.enabled = false;
    }

    public void mimicRaw(Vector2 val)
    {
        vector_float_handle.anchoredPosition = val;
    }

    public void mimicPress(bool press)
    {
        icon_press.enabled = press;
    }

    public void mimicPunch(Vector2 direction)
    {
        vector_punchs.Find("N").gameObject.SetActive(direction.y > 0f);
        vector_punchs.Find("E").gameObject.SetActive(direction.x > 0f);
        vector_punchs.Find("S").gameObject.SetActive(direction.y < 0f);
        vector_punchs.Find("W").gameObject.SetActive(direction.x < 0f);
    }

    public void mimicDirection(Vector2 direction)
    {
        vector_ints.Find("N").gameObject.SetActive(direction.y > 0f);
        vector_ints.Find("E").gameObject.SetActive(direction.x > 0f);
        vector_ints.Find("S").gameObject.SetActive(direction.y < 0f);
        vector_ints.Find("W").gameObject.SetActive(direction.x < 0f);
    }

}
