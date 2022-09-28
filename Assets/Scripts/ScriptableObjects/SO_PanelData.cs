using UnityEngine;
using System.Linq;

namespace TI4
{
    [CreateAssetMenu(menuName = "UI/PanelData", fileName = "PanelData")]
    public class SO_PanelData : ScriptableObject
    {
        [SerializeField]
        private UIPanel[] panels;

        public UIPanel GetPanel(UI.PanelType panelType)
        {
            return panels.FirstOrDefault(panel => panel.PanelType == panelType);
        }
    }
}