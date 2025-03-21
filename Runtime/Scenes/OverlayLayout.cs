using UnityEngine;
namespace fwp.gamepad.layout
{

    public class OverlayLayout : MonoBehaviour
    {
        public DataGamepadLayout layout;

        public Transform pads;
        public Transform dpads;

        public bool apply = false;

        private void OnValidate()
        {
            applyLayout(layout);

            apply = false;
        }

        void applyLayout(DataGamepadLayout layout)
        {
            this.layout = layout;
            if (this.layout == null)
                return;

            Debug.Log("oly.apply "+layout.name, layout);

            if (pads != null)
            {
                foreach(var elmt in layout.getActions()) applyField<InputButtons>(elmt);
            }

            if(dpads != null)
            {
                foreach (var elmt in layout.getDpads()) applyField<InputDPad>(elmt);
            }
        }

        void applyField<T>(LayoutInput<T> settings)
        {
            Debug.Log(settings.input);
            Transform target = pads.transform.Find(settings.input.ToString().ToLower());
            applyField(target, settings as LayoutInputAction);
        }

        void applyField(Transform pivot, LayoutInputAction settings)
        {
            if (pivot == null) return;
            pivot.Find("icon").GetComponent<UnityEngine.UI.Image>().sprite = settings.getIcon();
            pivot.Find("label").GetComponent<TMPro.TextMeshProUGUI>().SetText(settings.getLabel());
        }
    }

}