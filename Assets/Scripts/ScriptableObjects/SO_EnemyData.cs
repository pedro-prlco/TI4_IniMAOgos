using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TI4
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
    public class SO_EnemyData : ScriptableObject
    {
        public int Dificuldade;
        public string word;
        public float speed;
    }
}

