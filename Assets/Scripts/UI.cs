using UnityEngine;
using System;

namespace TI4
{
    public class UI : MonoBehaviour
    {

        public event Action OnPanelInstantiated;

        public enum PanelType
        {
            MainMenu,
            Customize,
            Config,
            Map,
        }

        [SerializeField]
        Transform panelHolder;
        [SerializeField]
        SO_PanelData panelData;

        UIPanel currentPanel;

        public void Display_MainMenu()
        {
            SetPanel<UIPanel_MainMenu>(PanelType.MainMenu);
        }

        public void Display_Customize()
        {
            SetPanel<UIPanel_Customize>(PanelType.Customize);
        }

        public Panel SetPanel<Panel>(PanelType panelType)
            where Panel : UIPanel
        {

            if(currentPanel != null && currentPanel.PanelType == panelType)
            {
                return currentPanel as Panel;
            }
            
            foreach(Transform panel in panelHolder)
            {
                GameObject.Destroy(panel.gameObject);
            }

            return InstantiatePanel<Panel>(panelType);
        }

        Panel InstantiatePanel<Panel>(PanelType panelType)
            where Panel : UIPanel
        {
            var panel = (Panel)Instantiate(panelData.GetPanel(panelType), panelHolder);
            OnPanelInstantiated?.Invoke();
            currentPanel = panel;
            return panel;
        }
    }
}