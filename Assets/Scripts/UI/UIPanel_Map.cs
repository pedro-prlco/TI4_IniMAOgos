using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace TI4
{
    public class UIPanel_Map : UIPanel
    {
        [SerializeField] private GameObject _warningAboutEasyPath;
        [SerializeField] Button yesButton;
        [SerializeField] Button noButton;
        [SerializeField] TextMeshProUGUI warningLabel;

        [SerializeField] RectTransform levelHolderContainer;
        [SerializeField] TextMeshProUGUI levelHolderTitleLbl;
        [SerializeField] TextMeshProUGUI levelDescriptionLbl;

        [SerializeField] RectTransform playButton;
        [SerializeField] Button playButton_Button;

        [SerializeField] float holderXOffset;

        bool inGame;

        string warning = "Você vai jogar a fase {0} depois de ter feito um caminho fácil no seu trajeto. Isso resulta em uma gameplay mais difícil.";

        Vector3 levelHolderPosition;
        Vector2 screenPosition;

        void Start()
        {
            levelHolderPosition = Vector3.one * -1f; ;
            yesButton.onClick.AddListener(() => { Debug.Log("yes !"); _warningAboutEasyPath.SetActive(false); LevelGraph.CanInteract = true; MapCameraController.CanInteract = true; });
            noButton.onClick.AddListener(() => { Debug.Log("no !"); _warningAboutEasyPath.SetActive(false); LevelGraph.CanInteract = true; MapCameraController.CanInteract = true; });
            playButton_Button.onClick.AddListener(() => 
            { 
                if(!inGame)
                {
                    inGame = true;
                    Game.CurrentMatch = new Game.Match();

                    float camX = Game.GetLevelCamera().transform.position.x;
                    float camY = Game.GetLevelCamera().transform.position.y;
                    float camZ = Game.GetLevelCamera().transform.position.z;

                    float pX = Game.GetMainCharacter().transform.position.x;
                    float pY = Game.GetMainCharacter().transform.position.y;
                    float pZ = Game.GetMainCharacter().transform.position.z;

                    PlayerPrefs.SetFloat("camX", camX);
                    PlayerPrefs.SetFloat("camY", camY);
                    PlayerPrefs.SetFloat("camZ", camZ);
                    
                    PlayerPrefs.SetFloat("pX", pX);
                    PlayerPrefs.SetFloat("pY", pY);
                    PlayerPrefs.SetFloat("pZ", pZ);

                    PlayerPrefs.SetFloat("Vertex", LevelGraph.CurrentVertex);

                    Game.LoadScene("Teste_de_Inimigo");
                }
                else
                {
                    inGame = false;
                    Game.LoadScene("Mapa");
                } 
            });
        }

        void Update()
        {
            if (levelHolderPosition != Vector3.one * -1)
            {
                screenPosition = Input.mousePosition;

                screenPosition += Vector2.right * holderXOffset;

                float clampedX = Mathf.Clamp(screenPosition.x, levelHolderContainer.sizeDelta.x / 2, Screen.width - (levelHolderContainer.sizeDelta.x / 2));
                float clampedY = Mathf.Clamp(screenPosition.y, levelHolderContainer.sizeDelta.y / 2, Screen.height - (levelHolderContainer.sizeDelta.y / 2));

                screenPosition = new Vector2(clampedX, clampedY);

                levelHolderContainer.transform.position = screenPosition;
                levelHolderContainer.gameObject.SetActive(true);
            }
            else
            {
                levelHolderContainer.gameObject.SetActive(false);
            }
        }

        public void DisplayWarning()
        {
            warningLabel.text = string.Format(warning, LevelInfo.current.Title);
            _warningAboutEasyPath.SetActive(true);
            Game.GetMainCharacter().SetState(CharacterBase.State.Spotted);
            LevelGraph.CanInteract = false;
            MapCameraController.CanInteract = false;
        }

        public void SetLevelHover(string title, string description, Vector3 worldPosition)
        {
            levelHolderTitleLbl.text = title;
            levelDescriptionLbl.text = description;
            levelHolderPosition = worldPosition;
        }

        public void DisplayPlayButton()
        {
            playButton.DOAnchorPos(Vector3.zero, .3f).SetEase(Ease.InCubic);
        }
        
        public void HidePlayButton()
        {
            playButton.DOAnchorPos(new Vector3(0, -150, 0), .3f);
        }

        public void HideLevelHolder()
        {
            levelHolderPosition = Vector3.one * -1;
        }

        bool ContainsMouse()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3[] corners = new Vector3[4];
            levelHolderContainer.GetWorldCorners(corners);

            return (corners[0].x <= mousePosition.x && corners[2].x >= mousePosition.x) && (corners[0].y <= mousePosition.y && corners[2].y >= mousePosition.y);
        }
    }
}