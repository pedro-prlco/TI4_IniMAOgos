using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        [SerializeField] float holderXOffset;

        string warning = "Você vai jogar a fase {0} depois de ter feito um caminho fácil no seu trajeto. Isso resulta em uma gameplay mais difícil. Aceita o desafio ?";

        Vector3 levelHolderPosition;
        Vector2 screenPosition;

        void Start()
        {
            levelHolderPosition = Vector3.one * -1f; ;
            yesButton.onClick.AddListener(() => { Debug.Log("yes !"); _warningAboutEasyPath.SetActive(false); LevelGraph.CanInteract = true; MapCameraController.CanInteract = true; });
            noButton.onClick.AddListener(() => { Debug.Log("no !"); _warningAboutEasyPath.SetActive(false); LevelGraph.CanInteract = true; MapCameraController.CanInteract = true; });
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