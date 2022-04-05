using UnityEngine;

namespace HNC {
    public class EnemySuspiciousState : EnemyState {
        public EnemySuspiciousState(EnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter() {
            base.Enter();
            if (_enemy.HasAnimator) {
                _enemy.AnimatorComponent.SetTrigger(_enemy.AnimScreamHash);
            }
            _enemy.SuspiciousTimer = _enemy.SuspiciousTime;
            _enemy.NavMeshAgent.destination = _enemy.Detected.transform.position;
            if (_enemy.HasAnimator) {
                _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
            }
        }

        public override void Exit() {
            _enemy.SuspiciousTimer = _enemy.SuspiciousTime + 1;
        }

        public override void Update() {
            _enemy.SuspiciousTimer -= Time.deltaTime;
            if (_enemy.NavMeshAgent.remainingDistance < _enemy.SuspiciousTreshoold && _enemy.HasAnimator) {
                //TODO Look around
                _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
            }
        }

    }
}
