using UnityEngine;

namespace HNC {
    public class EnemySuspiciousState : IState {
        private readonly EnemyController _enemy;

        public EnemySuspiciousState(EnemyController enemy) => _enemy = enemy;

        public void Enter() {
            _enemy.SuspiciousTimer = _enemy.SuspiciousTime;
            _enemy.NavMeshAgent.SetDestination(_enemy.detected.transform.position);
            if (_enemy.HasAnimator) {
                _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
            }
        }

        public void Exit() => _enemy.SuspiciousTimer = _enemy.SuspiciousTime;

        public void Update() {
            _enemy.SuspiciousTimer -= Time.deltaTime;
            if (_enemy.NavMeshAgent.remainingDistance < _enemy.PatrolTreshoold && _enemy.HasAnimator) {
                _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
            }
        }

    }
}
