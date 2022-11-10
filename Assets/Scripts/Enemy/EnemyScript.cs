using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TI4
{
    public class EnemyScript : MonoBehaviour
    {

        public int Dificuldade => data[dificuldadeId].Dificuldade;
        
        int dificuldadeId;
        public int id;
        [SerializeField] private float speed = 1f;
        [SerializeField] private string word = "Teste";
        [SerializeField] private TextMeshPro label;
        [SerializeField] private SO_EnemyData[] data;
        private string typed = "";
        private GameObject target;

        // Start is called before the first frame update
        void Start()
        {
            SetEnemyValues();
        }

        private void SetEnemyValues()
        {
            dificuldadeId = Random.Range(0, data.Length);
            this.word = data[dificuldadeId].word.ToLower();
            this.speed = data[dificuldadeId].speed;

            label.text = word;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            if (Input.anyKeyDown)
            {
                SetWord();
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
                Destroy(gameObject);
                EnemySpawner.EnemiesInScreen.Remove(this);
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<HealthTest>() != null)
                {
                    other.GetComponent<HealthTest>().Damage(1);
                    EnemySpawner.EnemiesInScreen.Remove(this);
                    Destroy(gameObject);
                }
            }
        }
    }
}
