using UnityEngine;

namespace TI4
{
    public class OpenPanelOnStart : MonoBehaviour
    {
        public UI.PanelType Type;

        void Start()
        {
            switch(Type)
            {
                case UI.PanelType.MainMenu:
                    Game.GetUIController().SetPanel<UIPanel_MainMenu>(UI.PanelType.MainMenu);
                break;
                case UI.PanelType.Customize:
                    Game.GetUIController().SetPanel<UIPanel_Customize>(UI.PanelType.Customize);
                break;
                case UI.PanelType.Config:
                    Game.GetUIController().SetPanel<UIPanel_Config>(UI.PanelType.Config);
                break;
                case UI.PanelType.Map:
                    Game.GetUIController().SetPanel<UIPanel_Map>(UI.PanelType.Map);
                break;
                case UI.PanelType.Gameplay:
                    Game.GetUIController().SetPanel<UIPanel_Gameplay>(UI.PanelType.Gameplay);
                    break;
            }
        }
    }
}