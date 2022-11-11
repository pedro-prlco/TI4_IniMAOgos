using System.Collections;
using UnityEngine;
using System.Collections.Generic;

using TI4.IA;

namespace TI4
{
    public class EnemySpawner : MonoBehaviour
    {
        public static List<EnemyScript> EnemiesInScreen;
        public static int TotalSpawned;

        [SerializeField] private EnemyScript enemyPrefab1;
        [SerializeField] private float spawnFrequency1 = 7f;

        [SerializeField] private EnemyScript enemyPrefab2;
        [SerializeField] private float spawnFrequency2 = 5f;

        [SerializeField] private EnemyScript enemyPrefab3;
        [SerializeField] private float spawnFrequency3 = 11f;

        private float interval;

        // Start is called before the first frame update
        void Start()
        {
            interval = Random.Range(2, 5);

            if (EnemiesInScreen == null || EnemiesInScreen.Count > 0)
            {
                EnemiesInScreen = new List<EnemyScript>();
            }

            if (TotalSpawned > 0)
            { 
                TotalSpawned = 0; 
            }

            StartCoroutine(Spawn(spawnFrequency1));
    
            Game.Match.OnEndGame += (state)=> 
            {
                for(int i = 0; i < EnemiesInScreen.Count; i++)
                {
                    Destroy(EnemiesInScreen[i].gameObject);
                }
            };
        }

        private IEnumerator Spawn(float interval)
        {
            while (TotalSpawned < LevelInfo.currentData.MaxEnemies)
            {
                yield return new WaitForSeconds(interval);

                interval = Random.Range(2, 5);

                EnemyScript[] enemies = new EnemyScript[]
                {
                    enemyPrefab1,
                    enemyPrefab2,
                    enemyPrefab3
                };
                
                if(EnemiesInScreen.Count < LevelInfo.currentData.MaxEnemiesInScreen)
                {
                    int enemyId = Random.Range(0, LevelInfo.currentData.EnemyLvl.Length - 1);

                    DecisionTreeManager.instance.Setup(out int level);

                    if(level > 0)
                    {
                        Debug.Log("Predicted level: " + level);

                        EnemyScript newEnemy = Instantiate(enemies[level - 1], transform.position, Quaternion.identity);
                        newEnemy.id = EnemiesInScreen.Count;

                        EnemiesInScreen.Add(newEnemy);
                        
                        TotalSpawned++;
                    }
                }
            }

            while(TotalSpawned >= LevelInfo.currentData.MaxEnemies && EnemiesInScreen.Count > 0)
            {
                yield return new WaitForSeconds(1f);
            }

            Game.CurrentMatch.EndMatch(Game.Match.EndMatchState.WIN);
        }
    }
}
