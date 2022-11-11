using UnityEngine;

using UnityEngine.UI;

using System.Collections.Generic;
using System.Linq;

namespace TI4
{
    public class UIElement_LibraDisplay : MonoBehaviour
    {
        public char[] CurrentChars => wordChars.ToArray();

        [SerializeField] LibraChar[] chars;
        [SerializeField] Image charPrefab;
        [SerializeField] HorizontalLayoutGroup holder;
        [SerializeField] Color markedColor;

        List<char> wordChars;

        string currentWord;

        List<Image> charImages;
        
        private void Start() 
        {
            charPrefab.gameObject.SetActive(false);
        }

        void Update()
        {
            if (Input.anyKeyDown && wordChars != null)
            {
                char lastChar = Input.inputString[Input.inputString.Length -1];
                if(wordChars.Contains(lastChar))
                {
                    int index = wordChars.IndexOf(lastChar);
                    charImages[index].color = markedColor;
                    charImages.RemoveAt(index);
                    wordChars.Remove(lastChar);
                }
            }
        }

        public void SetWord(string word)
        {
            word = word.ToLower();

            wordChars = new List<char>();
            foreach(char c in word)
            {
                wordChars.Add(c);
            }

            charImages = new List<Image>();
            
            int id = 0;
            foreach(char letter in word)
            {
                Image charClone = Instantiate(charPrefab, holder.transform);
                charClone.gameObject.SetActive(true);
                charClone.sprite = chars.Where(data => data.Char[0] == letter).FirstOrDefault().Sprite;
                charClone.name = ""+letter;
                charImages.Add(charClone);
                id++;
            }
        }

        public void MarkCharId(int id)
        {
            charImages[id].color = markedColor;
        }
    }

    [System.Serializable]
    public struct LibraChar
    {
        public string Char;
        public Sprite Sprite;
    }
}