using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TI4
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab1;
        [SerializeField] private float spawnFrequency1 = 7f;

        [SerializeField] private GameObject enemyPrefab2;
        [SerializeField] private float spawnFrequency2 = 5f;

        [SerializeField] private GameObject enemyPrefab3;
        [SerializeField] private float spawnFrequency3 = 11f;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Spawn(spawnFrequency1, enemyPrefab1));
            StartCoroutine(Spawn(spawnFrequency2, enemyPrefab2));
            StartCoroutine(Spawn(spawnFrequency3, enemyPrefab3));
        }

        private IEnumerator Spawn(float interval, GameObject enemy)
        {
            yield return new WaitForSeconds(interval);
            if (GameObject.FindGameObjectWithTag("Finish") != null)
            {
                GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
                StartCoroutine(Spawn(interval, enemy));
            }
        }
    }
}
