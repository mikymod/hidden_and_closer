using UnityEngine;

namespace HNC
{
    class PlayerDeath : IState
    {
        private PlayerController player;

        public PlayerDeath(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.Animator.SetTrigger(player.DeathParamName);
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }
}