using UnityEngine;

namespace TI4
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        CharacterBase _characterBaseReference;
        
        void Awake()
        {
            _characterBaseReference = transform.parent.gameObject.GetComponent<CharacterBase>();
        }
        
        void Start()
        {
            _characterBaseReference.OnStateChanged += (state) => { PlayAnimation(state.ToString()); };
        }

        void PlayAnimation(string state)
        {
            switch(state.ToLower())
            {
                case "idle":
                    _animator.SetBool("walk", false);
                    _animator.SetBool("run", false);
                    break;
                case "walk":
                    _animator.SetBool("walk", true);
                    _animator.SetBool("run", false);
                    break;
                case "run":
                    _animator.SetBool("run", true);
                    break;
                case "spotted":
                    _animator.SetBool("spotted", true);
                    _animator.SetBool("run", false);
                    break;
            }
        }
    }
}