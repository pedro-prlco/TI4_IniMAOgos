using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TI4
{
    public class HealthTest : MonoBehaviour
    {
        [SerializeField] private int health = 5;
        private int MAX_HEALTH = 7;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void SetHealth(int health, int maxHealth)
        {
            this.health = health;
            this.MAX_HEALTH = maxHealth;
        }

        public void Damage(int amount)
        {
            if (amount < 0)
            {
                throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
            }
            this.health -= amount;
            if (health <= 0)
            {
                Death();
            }
        }

        public void Heal(int amount)
        {
            if (amount < 0)
            {
                throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
            }
            bool overhealing = this.health + amount > MAX_HEALTH;
            if (overhealing)
            {
                this.health = MAX_HEALTH;
            }
            else
            {
                this.health += amount;
            }
        }

        private void Death()
        {
            Debug.Log("Você morreu");
            Destroy(gameObject);
        }
    }
}
