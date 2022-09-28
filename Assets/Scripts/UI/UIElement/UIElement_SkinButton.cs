using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TI4
{
    public class UIElement_SkinButton : MonoBehaviour
    {
        [SerializeField]
        private Image _skinIcon;
        [SerializeField]
        private TextMeshProUGUI _skinLabel;
        [SerializeField]
        private Button _skinButton;

        private CharacterSkinData _skin;

        public void Setup(CharacterSkinData skin)
        {
            _skin = new CharacterSkinData(skin.Name, skin.SkinMaterial, skin.StorePresence);
            _skinIcon.sprite = _skin.StorePresence;
            _skinLabel.text = _skin.Name;

            _skinButton.onClick.RemoveAllListeners();
            _skinButton.onClick.AddListener(()=> { Game.SetMainCharacterSkin(_skin); });
        }
    }
}