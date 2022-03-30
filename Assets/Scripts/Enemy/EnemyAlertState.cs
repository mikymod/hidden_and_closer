using UnityEngine;

namespace HNC {
    public class EnemyAlertState : IState {
        private readonly EnemyController _enemy;

        public EnemyAlertState(EnemyController enemy) => _enemy = enemy;

        public void Enter() {
            _enemy.AlertTimer = _enemy.AlertTime;
            _enemy.AlertGO.SetActive(true);
            CheckDistance();
        }

        public void Exit() {
            _enemy.AlertGO.SetActive(false);
            _enemy.AlertTimer = _enemy.AlertTime + 1;
        }

        public void Update() {
            _enemy.AlertTimer -= Time.deltaTime;
            if (_enemy.NavMeshAgent.remainingDistance <= _enemy.AlertTreshoold) {
                if (_enemy.HasAnimator) {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
                }
            }
            CheckDistance();

        }

        private void CheckDistance() {
            if ((_enemy.VideoDetected.transform.position - _enemy.transform.position).sqrMagnitude > _enemy.AlertTreshoold * _enemy.AlertTreshoold) {
                _enemy.NavMeshAgent.destination = _enemy.VideoDetected.transform.position;
                if (_enemy.HasAnimator) {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
                }
            }
        }
    }
}
