using UnityEngine;
using UnityEngine.Events;

namespace TI4
{
    public class Game : MonoBehaviour
    {
        private static Game instance;

        [SerializeField]
        private UI _ui;

        [SerializeField]
        private SO_SkinData skinsData;

        [SerializeField]
        private UnityEvent OnStart;

        [SerializeField]
        CharacterBase _mainCharacter;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }

        void Start()
        {
            OnStart?.Invoke();
        }

        void SetMainCharacterInternal(CharacterBase characterBase)
        {
            _mainCharacter = characterBase;
        }

        void SetMainCharacterSkinInternal(CharacterSkinData skin)
        {
            if(_mainCharacter == null)
            {
                Debug.Log("Main Character is not defined. Use SetMainCharacter() and call it again.");
                return;
            }

            _mainCharacter.SetSkin(skin);
        }

        CharacterSkinData[] GetAvailableCharacterSkinsInternal()
        {
            return skinsData.AllSkins();
        }

        CharacterSkinData GetCharacterSkinInternal(string name)
        {
            return skinsData.GetSkin(name);
        }

        public static CharacterSkinData[] GetAvailableCharacterSkins()
        {
            if(instance == null)
            {
                return null;
            }

            return instance.GetAvailableCharacterSkinsInternal();
        }

        public static CharacterSkinData GetCharacterSkin(string name)
        {
            if(instance == null)
            {
                return CharacterSkinData.None;
            }

            return instance.GetCharacterSkinInternal(name);
        }

        public static void SetMainCharacterSkin(CharacterSkinData skin)
        {
            if(instance == null)
            {
                return;
            }

            instance.SetMainCharacterSkinInternal(skin);
        }

        public static void SetMainCharacter(CharacterBase characterBase)
        {
            if(instance == null)
            {
                return;
            }

            instance.SetMainCharacterInternal(characterBase);
        }

        public static UI GetUIController()
        {
            if(instance == null || instance._ui == null)
            {
                return null;
            }

            return instance._ui;
        }
    }
}