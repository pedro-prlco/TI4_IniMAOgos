using System;
using UnityEngine;

namespace TI4
{
    [System.Serializable]
    public struct CharacterSkinData
    {
        public string Name;
        public Material SkinMaterial;
        public Sprite StorePresence;

        public static CharacterSkinData None => new CharacterSkinData("none", null, null);

        public CharacterSkinData(string name, Material skin, Sprite storePresence)
        {
            Name = name;
            SkinMaterial = skin;
            StorePresence = storePresence;
        }
    }
}