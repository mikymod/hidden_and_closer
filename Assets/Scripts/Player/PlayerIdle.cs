using UnityEngine;

namespace HNC
{

    public class PlayerIdle : IState
    {
        private readonly PlayerController _player;
        public PlayerIdle(PlayerController player) => _player = player;

        public void Enter()
        {
            if (_player.HasAnimator)
            {
                _player.Animator.SetBool(_player.MoveParamName, false);
            }
        }
        void IState.Update()
        {
            // Debug.Log("Player Idle Update");
            _player.Animator.SetBool(_player.CrouchParamName, _player.IsCrouching);
        }

        public void Exit()
        {
            // Debug.Log("Player Idle Exit");
        }
    }
}
