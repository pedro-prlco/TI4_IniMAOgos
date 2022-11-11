using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

using System;

namespace TI4
{
    public class Game : MonoBehaviour
    {
        private static Game instance;

        public static Match CurrentMatch;

        [SerializeField]
        private UI _ui;

        [SerializeField]
        private SO_SkinData skinsData;

        [SerializeField]
        private UnityEvent OnStart;

        CharacterBase _mainCharacter;
        Camera levelCamera;

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
            OnStart.RemoveAllListeners();
        }

        CharacterBase GetMainCharacterInternal()
        {
            return _mainCharacter;
        }

        void SetMainCharacterInternal(CharacterBase characterBase)
        {
            _mainCharacter = characterBase;
            _mainCharacter.SetSkin(skinsData.GetSkin(PlayerPrefs.GetString("CurrentSkin", "Padrão")));
        }

        void SetMainCharacterSkinInternal(CharacterSkinData skin)
        {
            if(_mainCharacter == null)
            {
                Debug.Log("Main Character is not defined. Use SetMainCharacter() and call it again.");
                return;
            }

            _mainCharacter.SetSkin(skin);
            PlayerPrefs.SetString("CurrentSkin", skin.Name);
        }

        CharacterSkinData[] GetAvailableCharacterSkinsInternal()
        {
            return skinsData.AllSkins();
        }

        CharacterSkinData GetCharacterSkinInternal(string name)
        {
            return skinsData.GetSkin(name);
        }

        public void LoadSceneInternal(string scene)
        {
            SceneManager.LoadScene(scene);
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

        public static Camera GetLevelCamera()
        {
            if(instance == null)
            {
                return Camera.main;
            }

            return instance.levelCamera;
        }

        public static void SetLevelCamera(Camera camera)
        {
            if(instance == null)
            {
                return;
            }

            instance.levelCamera = camera;
        }

        public static void LoadScene(string sceneName)
        {
            if(instance == null)
            {
                return;
            }

            instance.LoadSceneInternal(sceneName);
        }

        public static CharacterBase GetMainCharacter()
        {
            if(instance == null)
            {
                return null;
            }

            return instance.GetMainCharacterInternal();
        }

        public class Match
        {

            public enum EndMatchState { WIN, LOSE }

            public static Action<int> OnLifeChanged;
            public static Action<float> OnScoreChanged;
            public static Action<EndMatchState> OnEndGame;

            private float score;
            private int life;

            public float Score => score;
            public int Life => life;

            public Match()
            {
                life = 5;
            }

            public void SetLife(int newLife)
            {
                life = newLife;

                if(life == 0)
                {
                    EndMatch(EndMatchState.LOSE);
                }

                OnLifeChanged?.Invoke(life);
            }

            public void SetScore(float newScore)
            {
                score = newScore;
                OnScoreChanged?.Invoke(newScore);
            }

            public void EndMatch(EndMatchState state)
            {
                float bestScore = PlayerPrefs.GetFloat("bestScore", 0);

                if(bestScore < score)
                {
                    PlayerPrefs.SetFloat("bestScore", score);
                }

                OnEndGame?.Invoke(state);
            }
        }
    }
}