using UnityEngine;
using HNC.Audio;

namespace HNC {
    public class NewEnemyAlertState : NewEnemyState {
        public NewEnemyAlertState(NewEnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter() {
            base.Enter();
            _enemy.AlertTimer = _enemy.AlertTime;
            _enemy.transform.LookAt(_enemy.PosToGo, Vector3.up);
            if (_enemy.TryGetComponent(out AudioZombieController component))
            {
                component.PlayAlertSound();
            }
        }

        public override void Exit() => _enemy.TransitionToAlertState = false;

        public override void Update() {
            _enemy.NavMeshAgent.SetDestination(_enemy.PosToGo);
            _enemy.Animator.SetFloat("Speed", 1);
            _enemy.AlertTimer -= Time.deltaTime;
            if (_enemy.AlertTimer <= 0) {
                _enemy.TransitionToIdleState = true;
            }
            if (_enemy.NavMeshAgent.remainingDistance <= _enemy.NavMeshAgent.stoppingDistance) {
                _enemy.Animator.SetFloat("Speed", 0);
            }
        }
    }
}
