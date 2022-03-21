using UnityEngine;

namespace HNC {

    public class PlayerIdle : IState {
        private readonly PlayerController _player;
        public PlayerIdle(PlayerController player) => _player = player;

        public void Enter() {
            if (_player.HasAnimator) {
                _player.Animator.SetBool(_player.AnimatorMoveHash, false);
            }
        }
        void IState.Update() => Debug.Log("Player Idle Update");
        public void Exit() => Debug.Log("Player Idle Exit");
    }
}
