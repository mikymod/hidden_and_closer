using UnityEngine;

namespace HNC
{
    public class NewEnemyAlertState : NewEnemyState
    {
        public NewEnemyAlertState(NewEnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter()
        {
            base.Enter();
            _enemy.AlertTimer = _enemy.AlertTime;
        }

        public override void Exit()
        {
            _enemy.TransitionToAlertState = false;
        }

        public override void Update()
        {
            _enemy.NavMeshAgent.destination = _enemy.PosToGo;
            _enemy.AlertTimer -= Time.deltaTime;
            if (_enemy.AlertTimer <= 0)
            {
                _enemy.TransitionToIdleState = true;
            }
            if (_enemy.NavMeshAgent.remainingDistance <= _enemy.AlertTreshoold)
            {
                //Stop Zombie
            }
        }
    }
}
