using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;

namespace TI4
{
    public class UIPanel_Config : UIPanel
    {
        [SerializeField]
        private Button _btnClose;

        [SerializeField]
        private Slider _sliderGeneral;
        [SerializeField]
        private Slider _sliderMusic;
        [SerializeField]
        private Slider _sliderEffects;

        public void onGeneralChange(float value){
            Debug.Log($"Valor Geral: {value}");
        }
        public void onMusicChange(float value){
            Debug.Log($"Valor Musica: {value}");
        }
        public void onEffectsChange(float value){
            Debug.Log($"Valor Efeitos: {value}");
        }

        void Start()
        {
            _btnClose.onClick.AddListener(()=> { Game.GetUIController().SetPanel<UIPanel_MainMenu>(UI.PanelType.MainMenu); });
        }

        void Update()
        {
            
        }
    }
}