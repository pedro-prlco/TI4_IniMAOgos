using UnityEngine;

namespace TI4
{
    public class SetAsMainOnStart : MonoBehaviour
    {
        enum MainType { Character, Camera }

        [SerializeField] MainType type;
        [SerializeField] bool loadPos;

        void Start()
        {
            switch (type)
            {
                case MainType.Character:
                    CharacterBase character = GetComponent<CharacterBase>();
                    if (character != null)
                    {
                        Game.SetMainCharacter(character);

                    }
                    
                    if (loadPos)
                    {
                        float pX = PlayerPrefs.GetFloat("pX", -1);
                        float pY = PlayerPrefs.GetFloat("pY");
                        float pZ = PlayerPrefs.GetFloat("pZ");
                        if (pX != -1)
                            transform.position = new Vector3(pX, pY, pZ);
                    }
                    break;
                case MainType.Camera:
                    Camera camera = GetComponent<Camera>();
                    if (camera != null)
                    {
                        Game.SetLevelCamera(camera);
                    }

                    if(loadPos)
                    {
                         float cX = PlayerPrefs.GetFloat("camX", -1);
                        float cY = PlayerPrefs.GetFloat("camY");
                        float cZ = PlayerPrefs.GetFloat("camZ");

                         if (cX != -1)
                                transform.position = new Vector3(cX, cY, cZ);
                    }
                    break;
            }
        }
    }
}