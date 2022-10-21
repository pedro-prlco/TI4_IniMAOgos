using UnityEngine;

namespace TI4
{
    public class LevelInfo : MonoBehaviour
    {
        public static LevelInfo current;
        public static bool CanInteract = true;

        public string Title => title;
        
        [SerializeField] string title;
        [SerializeField, TextArea] string description;
        [SerializeField] LayerMask targetLayer;

        void Update()
        {
            if(!CanInteract)
            {
                return;
            }

            Camera levelCamera = Game.GetLevelCamera();
            Ray ray = levelCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, 1000, targetLayer))
            {
                if(hit.transform == transform)
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        current = this;
                    }
                    Game.GetUIController().SetPanel<UIPanel_Map>(UI.PanelType.Map).SetLevelHover(title, description, transform.localPosition);
                }
            }
            else
            {
                Game.GetUIController().SetPanel<UIPanel_Map>(UI.PanelType.Map).HideLevelHolder();
            }
        }
    }
}