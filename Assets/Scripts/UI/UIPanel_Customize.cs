using UnityEngine;
using UnityEngine.UI;

namespace TI4
{
    public class UIPanel_Customize : UIPanel
    {

        [SerializeField]
        private UIElement_SkinButton _skinBtnPrefab;

        [SerializeField]
        private Transform _skinsHolder;

        [SerializeField]
        private Button _btnClose;

        void Start()
        {
            SpawnSkins();

            _btnClose.onClick.AddListener(()=> { Game.GetUIController().SetPanel<UIPanel_MainMenu>(UI.PanelType.MainMenu); });
        }

        void SpawnSkins()
        {
            CharacterSkinData[] skins = Game.GetAvailableCharacterSkins();
            foreach(CharacterSkinData skin in skins)
            {
                UIElement_SkinButton instantiatedBtn = Instantiate(_skinBtnPrefab, _skinsHolder);
                instantiatedBtn.Setup(skin);
            }

            _skinBtnPrefab.gameObject.SetActive(false);
        }
    }
}