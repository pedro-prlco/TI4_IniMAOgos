using UnityEngine;

namespace TI4
{
    public class UIPanel : MonoBehaviour
    {
        public UI.PanelType PanelType => _panelType;

        [SerializeField]
        private UI.PanelType _panelType;
        [SerializeField]
        private byte priority;
    }
}
