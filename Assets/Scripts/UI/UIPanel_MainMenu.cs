using UnityEngine;
using UnityEngine.UI;

namespace TI4
{
    public class UIPanel_MainMenu : UIPanel
    {
        [SerializeField]
        private Button _playBtn;
        [SerializeField]
        private Button _storeBtn;
        [SerializeField]
        private Button _configBtn;
        [SerializeField]
        private Button _quitBtn;
        
        void Start()
        {
            _storeBtn.onClick.RemoveAllListeners();
            _configBtn.onClick.RemoveAllListeners();
            _quitBtn.onClick.RemoveAllListeners();

            _storeBtn.onClick.AddListener(()=> { Game.GetUIController().SetPanel<UIPanel_Customize>(UI.PanelType.Customize); } );
            _configBtn.onClick.AddListener(()=> { Game.GetUIController().SetPanel<UIPanel_Config>(UI.PanelType.Config); } );
            _quitBtn.onClick.AddListener(Quit);
        }
        void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}