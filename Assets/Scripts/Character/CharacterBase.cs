using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TI4
{
    public class CharacterBase : MonoBehaviour
    {
        public Action<State> OnStateChanged;

        public enum State { Idle, Walk, Run, Spotted }

        private CharacterSkinData _currentSkin;

        [SerializeField]
        private State _currentState;

        public void SetState(State toState)
        {
            _currentState = toState;
            OnStateChanged?.Invoke(_currentState);
        }

        public void SetSkin(CharacterSkinData skin)
        {
            _currentSkin = new CharacterSkinData(skin.Name, skin.SkinMaterial, skin.StorePresence);
            GetComponentInChildren<SkinnedMeshRenderer>().material = _currentSkin.SkinMaterial;
            SetState(State.Spotted);
        }   
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CharacterBase))]
    class CharacterBase_CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(Application.isPlaying)
            {
                GUILayout.BeginHorizontal();
                {
                    for(int i = 0; i <= (int)CharacterBase.State.Spotted; i++)
                    {
                        if(GUILayout.Button(((CharacterBase.State)i).ToString()))
                        {
                            (target as CharacterBase).SetState((CharacterBase.State)i);
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
    }
#endif
}