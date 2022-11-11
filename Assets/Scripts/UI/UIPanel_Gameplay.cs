using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

using TMPro;

namespace TI4
{
    public class UIPanel_Gameplay : UIPanel
    {
        
        [SerializeField] GameObject heartsHolder;
        [SerializeField] GameObject[] hearts;
        [SerializeField] TextMeshProUGUI scoreLabel;

        [SerializeField] GameObject endGamePanel;
        [SerializeField] TextMeshProUGUI endGameLabel;
        [SerializeField] TextMeshProUGUI endGamescoreLabel;
        [SerializeField] TextMeshProUGUI endGamebestLabel;
        [SerializeField] Button backButton;

        [SerializeField] UIElement_LibraDisplay libraDisplayPrefab;
        [SerializeField] RectTransform wordsContainer;
        [SerializeField] GameObject dicasPanel;

        List<EnemyScript> enemies;
        List<UIElement_LibraDisplay> libras;

        [SerializeField] List<GameObject> tipChars;
        List<char> chars;

        Dictionary<char, GameObject> tipCharsDictionary;

        void Start()
        {
            
            tipCharsDictionary = new Dictionary<char, GameObject>();
            chars = new List<char>
            {
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
            };

            for(int i = 0; i < tipChars.Count; i++)
            {
                tipCharsDictionary.Add(chars[i], tipChars[i]);
            }

            Game.Match.OnLifeChanged += SetHearts;
            Game.Match.OnScoreChanged += SetScore;
            Game.Match.OnEndGame += OnEndGame;

            backButton.onClick.AddListener(()=> { UnityEngine.SceneManagement.SceneManager.LoadScene("Mapa"); });

            endGamePanel.SetActive(false);

            enemies = new List<EnemyScript>();
            libras = new List<UIElement_LibraDisplay>();
        }
        
        void Update()
        {

            foreach(EnemyScript enemy in EnemySpawner.EnemiesInScreen)
            {
                if(!enemies.Contains(enemy))
                {
                    var libraDisplay = Instantiate(libraDisplayPrefab, wordsContainer);
                    libraDisplay.gameObject.SetActive(true);
                    enemies.Add(enemy);
                    libras.Add(libraDisplay);
                    libraDisplay.SetWord(enemy.Word);
                }
            }

            List<int> wordsToRemove = new List<int>();
            List<char> allChars = new List<char>();

            int id = 0;
            foreach(var data in enemies)
            {
                
                if(data == null)
                {
                    wordsToRemove.Add(id);
                    id++;
                    continue;
                }

                Vector3 toPosition = Game.GetLevelCamera().WorldToScreenPoint(data.transform.position + (Vector3.up * 1.5f));
                allChars.AddRange(libras[id].CurrentChars);
                libras[id].transform.position = toPosition;
                id++;
            }

            foreach(var data in tipCharsDictionary)
            {
                data.Value.SetActive(allChars.Contains(data.Key));
            }

            for(int i = 0; i < wordsToRemove.Count;i++)
            {
                Destroy(libras[wordsToRemove[i]].gameObject);

                enemies.RemoveAt(wordsToRemove[i]);
                libras.RemoveAt(wordsToRemove[i]);
            }
        }

        void SetHearts(int life)
        {
            HideAll();
            for(int i = 0; i < life; i++)
            {
                hearts[i].SetActive(true);
            }
        }

        void SetScore(float score)
        {
            scoreLabel.text = "Score: " + score.ToString();
        }

        void HideAll()
        {
            foreach(GameObject heart in hearts)
            {
                heart.SetActive(false);
            }
        }

        void OnEndGame(Game.Match.EndMatchState state)
        {
            Game.Match.OnLifeChanged -= SetHearts;
            Game.Match.OnScoreChanged -= SetScore;
            Game.Match.OnEndGame -= OnEndGame;

            dicasPanel.SetActive(false);
            scoreLabel.gameObject.SetActive(false);
            heartsHolder.SetActive(false);

            endGamePanel.SetActive(true);

            endGamescoreLabel.text = "Pontos:\n" + Game.CurrentMatch.Score.ToString();
            endGamebestLabel.text = "Melhor pontuação:\n" + PlayerPrefs.GetFloat("bestScore").ToString();

            switch(state)
            {
                case Game.Match.EndMatchState.WIN:
                    endGameLabel.text = "Você ganhou !";
                    break;
                case Game.Match.EndMatchState.LOSE:
                    endGameLabel.text = "Você Perdeu";
                    endGameLabel.color = Color.red;
                    break;
            }
        }
    }
}