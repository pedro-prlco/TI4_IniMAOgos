using UnityEngine;
using UnityEngine.UI;

namespace TI4
{
    public class UIPanel_Map : UIPanel
    {
        [SerializeField] private GameObject _warningAboutEasyPath;
        [SerializeField] Button yesButton;
        [SerializeField] Button noButton;


        void Start()
        {
            yesButton.onClick.AddListener(()=> { Debug.Log("yes !"); _warningAboutEasyPath.SetActive(false); LevelGraph.CanInteract = true; MapCameraController.CanInteract = true; });
            noButton.onClick.AddListener(()=> { Debug.Log("no !"); _warningAboutEasyPath.SetActive(false); LevelGraph.CanInteract = true; MapCameraController.CanInteract = true; });
        }

        public void DisplayWarning()
        {
            _warningAboutEasyPath.SetActive(true);
            Game.GetMainCharacter().SetState(CharacterBase.State.Spotted);
            LevelGraph.CanInteract = false;
            MapCameraController.CanInteract = false;
        }    
    }
}