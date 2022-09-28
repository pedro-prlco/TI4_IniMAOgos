using UnityEngine;

namespace TI4
{
    public class SetAsMainOnStart : MonoBehaviour
    {
        void Start()
        {
            CharacterBase character = GetComponent<CharacterBase>();
            if(character != null)
            {
                Game.SetMainCharacter(character);
            }
        }
    }
}