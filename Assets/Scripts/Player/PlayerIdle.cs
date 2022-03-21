using UnityEngine;

namespace HNC {

    public class PlayerIdle : IState {
        private readonly Animator _animator;

        public PlayerIdle(Animator animator) {
            _animator = animator;
        }

        public void OnEnable() => Debug.Log("Player Idle Enable");
        public void Enter() {
            if (_animator != null) {
                _animator.SetBool("Move", false);
            }
        }
        void IState.Update() => Debug.Log("Player Idle Update");
        public void Exit() => Debug.Log("Player Idle Exit");
        public void OnDisable() => Debug.Log("Player Idle Disable");
    }
}
