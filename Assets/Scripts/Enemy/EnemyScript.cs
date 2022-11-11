using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TI4
{
    public class EnemyScript : MonoBehaviour
    {

        public int Dificuldade => data[dificuldadeId].Dificuldade;
        public string Word => word;

        int dificuldadeId;
        public int id;
        [SerializeField] private float speed = 1f;
        [SerializeField] private string word = "Teste";
        [SerializeField] private TextMeshPro label;
        [SerializeField] private SO_EnemyData[] data;
        
        List<char> wordChars;

        private string typed = "";
        private GameObject target;

        // Start is called before the first frame update
        void Start()
        {
            wordChars = new List<char>();
            label.gameObject.SetActive(false);
            SetEnemyValues();
        }

        private void SetEnemyValues()
        {
            dificuldadeId = Random.Range(0, data.Length);
            this.word = data[dificuldadeId].word.ToLower();
            this.speed = data[dificuldadeId].speed;

            foreach(char c in word)
            {
                wordChars.Add(c);
            }

            label.text = word;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            if (Input.anyKeyDown)
            {
                char lastChar = Input.inputString[Input.inputString.Length -1];
                if(wordChars.Contains(lastChar))
                {
                    wordChars.Remove(lastChar);
                    if(wordChars.Count == 0)
                    {
                        Game.CurrentMatch.SetScore(Game.CurrentMatch.Score + 10);
                        EnemySpawner.EnemiesInScreen.Remove(this);
                        Destroy(gameObject);
                    }
                }
            }

            float distance = Vector3.Distance(transform.position, Game.GetMainCharacter().transform.position);
            if(distance < 1.2f)
            {
                Game.GetMainCharacter().SetState(CharacterBase.State.Spotted);
                Game.CurrentMatch.SetLife(Game.CurrentMatch.Life - 1);
                EnemySpawner.EnemiesInScreen.Remove(this);
                Destroy(gameObject);
            }
        }

        private void SetWord()
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b') // backspace
                {
                    if (typed.Length != 0)
                    {
                        typed = typed.Substring(0, typed.Length - 1);
                    }
                }
                else if ((c == '\n') || (c == '\r')) // enter/return
                {
                    typed = "";
                }
                else
                {
                    typed += c;
                }
            }
            CheckWord();
        }

        private void CheckWord()
        {
            if (word.Equals(typed))
            {
                Game.CurrentMatch.SetScore(Game.CurrentMatch.Score + 10);
                EnemySpawner.EnemiesInScreen.Remove(this);
                Destroy(gameObject);
                typed = "";
            }
            else if (typed.Length > word.Length)
            {
                typed = "";
            }
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, Game.GetMainCharacter().transform.position, speed * Time.deltaTime);
        }
    }
}
