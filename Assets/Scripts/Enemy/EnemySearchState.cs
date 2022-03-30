using UnityEngine;

namespace HNC {
    public class EnemySearchState : IState {
        private readonly EnemyController _enemy;
        private Vector2 _randomPosition;

        public EnemySearchState(EnemyController enemy) => _enemy = enemy;

        public void Enter() {
            //_enemy.SearchGO.SetActive(true);
            _enemy.debugTest.text = "Search State";

            _enemy.SearchTimer = _enemy.SearchTime;
        }

        public void Update() {
            _enemy.SearchTimer -= Time.deltaTime;
            if (_enemy.NavMeshAgent.remainingDistance <= _enemy.PatrolTreshoold) {
                if (_enemy.detected != null) {
                    _enemy.NavMeshAgent.SetDestination(_enemy.detected.transform.position);
                } else {
                    _randomPosition = Random.insideUnitCircle * _enemy.PatrolRadius;
                    _enemy.NavMeshAgent.SetDestination(_enemy.transform.position + new Vector3(_randomPosition.x, 0, _randomPosition.y));
                }
            }
            if (_enemy.HasAnimator) {
                if (_enemy.NavMeshAgent.remainingDistance <= _enemy.PatrolTreshoold) {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
                } else {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
                }
            }
        }

        public void Exit() {

            //_enemy.SearchGO.SetActive(false);
            _enemy.SearchTimer = _enemy.SearchTime + 1;
        }
    }
}