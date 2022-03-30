using UnityEngine;

namespace HNC {
    public class EnemySuspiciousState : IState {
        private readonly EnemyController _enemy;

        public EnemySuspiciousState(EnemyController enemy) => _enemy = enemy;

        public void Enter() {
            if (_enemy.HasAnimator) {
                _enemy.AnimatorComponent.SetTrigger(_enemy.AnimScreamHash);
            }
            _enemy.SuspGO.SetActive(true);
            _enemy.SuspiciousTimer = _enemy.SuspiciousTime;
            _enemy.NavMeshAgent.SetDestination(_enemy.Detected.transform.position);
            if (_enemy.HasAnimator) {
                _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
            }
        }

        public void Exit() {
            _enemy.SuspGO.SetActive(false);
            _enemy.SuspiciousTimer = _enemy.SuspiciousTime + 1;
        }

        public void Update() {
            _enemy.SuspiciousTimer -= Time.deltaTime;
            if (_enemy.NavMeshAgent.remainingDistance < _enemy.SuspiciousTreshoold && _enemy.HasAnimator) {
                //TODO Look around
                _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
            }
        }

    }
}
