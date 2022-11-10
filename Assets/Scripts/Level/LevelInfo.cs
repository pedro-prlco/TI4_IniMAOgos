using UnityEngine;

using System.Linq;

namespace TI4
{
    public class LevelInfo : MonoBehaviour
    {
        public static LevelInfo current;
        public static SO_LevelData currentData;
        public static bool CanInteract = true;

        public string Title => title;
        
        [SerializeField] int levelType;
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
                        SO_LevelData[] allLevelDatas = Resources.LoadAll<SO_LevelData>("ScriptableObjects");
                        currentData = allLevelDatas.Where(data => data.Id == levelType).FirstOrDefault();
                        Debug.Log(currentData.ToString());
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