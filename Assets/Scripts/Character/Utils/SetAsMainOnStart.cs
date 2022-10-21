using UnityEngine;

namespace TI4
{
    public class SetAsMainOnStart : MonoBehaviour
    {
        enum MainType { Character, Camera }

        [SerializeField] MainType type;

        void Start()
        {
            switch(type)
            {
                case MainType.Character:
                    CharacterBase character = GetComponent<CharacterBase>();
                    if(character != null)
                    {
                        Game.SetMainCharacter(character);
                    }
                    break;
                case MainType.Camera:
                    Camera camera = GetComponent<Camera>();
                    if(camera != null)
                    {
                        Game.SetLevelCamera(camera);
                    }
                    break;
            }
        }
    }
}