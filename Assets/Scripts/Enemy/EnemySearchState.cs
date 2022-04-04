using UnityEngine;

namespace HNC {
    public class EnemySearchState : EnemyState {
        private Vector2 _randomPosition;

        public EnemySearchState(EnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter() {
            base.Enter();
            //_enemy.SearchTimer = _enemy.SearchTime;
        }

        public override void Update() {
            //_enemy.SearchTimer -= Time.deltaTime;
            if (_enemy.NavMeshAgent.remainingDistance <= _enemy.PatrolTreshoold) {
                if (_enemy.VideoDetected != null) {
                    _enemy.NavMeshAgent.destination = _enemy.VideoDetected.transform.position;
                } else {
                    _randomPosition = Random.insideUnitCircle * _enemy.PatrolRadius;
                    _enemy.NavMeshAgent.destination = _enemy.transform.position + new Vector3(_randomPosition.x, 0, _randomPosition.y);
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

        public override void Exit() { }//_enemy.SearchTimer = _enemy.SearchTime + 1;
    }
}