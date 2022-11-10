using UnityEngine;

namespace TI4
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "LevelData")]
    public class SO_LevelData : ScriptableObject
    {
        public int Id;
        
        public int MaxEnemiesInScreen;

        public int MinEnemies;
        public int MaxEnemies;
        
        public int[] EnemyLvl;

        public override string ToString()
        {
            return $"LevelData: {Id}";
        }
    }
}