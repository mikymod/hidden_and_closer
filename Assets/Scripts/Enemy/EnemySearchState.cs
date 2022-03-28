using UnityEngine;

namespace HNC {
    public class EnemySearchState : IState {
        private readonly EnemyController _enemy;

        public EnemySearchState(EnemyController enemy) => _enemy = enemy;

        public void Enter() => _enemy.SearchTimer = _enemy.SearchTime;

        public void Update() {
            _enemy.SearchTimer -= Time.deltaTime;
            _enemy.NavMeshAgent.SetDestination(_enemy.detected.transform.position);
            if (_enemy.HasAnimator) {
                if (_enemy.NavMeshAgent.remainingDistance <= _enemy.PatrolTreshoold) {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
                } else {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
                }
            }
        }

        public void Exit() => _enemy.SearchTimer = _enemy.SearchTime + 1;
    }
}