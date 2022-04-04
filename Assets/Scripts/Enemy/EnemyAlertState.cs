using UnityEngine;

namespace HNC {
    public class EnemyAlertState : EnemyState {
        public EnemyAlertState(EnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter() {
            base.Enter();
            _enemy.AlertTimer = _enemy.AlertTime;
            CheckDistance();
        }

        public override void Exit() {
            _enemy.AlertTimer = _enemy.AlertTime + 1;
        }

        public override void Update() {
            _enemy.AlertTimer -= Time.deltaTime;
            if (_enemy.NavMeshAgent.remainingDistance <= _enemy.AlertTreshoold) {
                if (_enemy.HasAnimator) {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
                }
            }
            CheckDistance();

        }

        private void CheckDistance() {
            if (_enemy.VideoDetected != null && (_enemy.VideoDetected.transform.position - _enemy.transform.position).sqrMagnitude > _enemy.AlertTreshoold * _enemy.AlertTreshoold) {
                _enemy.NavMeshAgent.destination = _enemy.VideoDetected.transform.position;
                if (_enemy.HasAnimator) {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
                }
            }
        }
    }
}
