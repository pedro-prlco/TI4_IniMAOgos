using UnityEngine;
using System.Linq;

namespace TI4
{
    [CreateAssetMenu(menuName = "SkinData", fileName = "SkinData")]
    public class SO_SkinData : ScriptableObject
    {
        [SerializeField]
        private CharacterSkinData[] skins;

        public CharacterSkinData GetSkin(string name)
        {
            return skins.First(skin => skin.Name == name);
        }

        public CharacterSkinData[] AllSkins()
        {
            return skins;
        }
    }
}