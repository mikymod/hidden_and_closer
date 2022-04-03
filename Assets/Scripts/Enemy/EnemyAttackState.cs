namespace HNC {
    public class EnemyAttackState : EnemyState {
        public EnemyAttackState(EnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter() {
            base.Enter();
            _enemy.AttackGO.SetActive(true);
            CheckDistance();
        }

        public override void Exit() => _enemy.AttackGO.SetActive(false);

        public override void Update() {
            if (_enemy.HasAnimator && _enemy.NavMeshAgent.remainingDistance <= _enemy.AttackTreshoold) {
                _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
                _enemy.AnimatorComponent.SetTrigger(_enemy.AnimAttackHash);
            }
            CheckDistance();
        }

        private void CheckDistance() {
            if ((_enemy.VideoDetected.transform.position - _enemy.transform.position).sqrMagnitude > _enemy.AttackTreshoold * _enemy.AttackTreshoold) {
                _enemy.NavMeshAgent.destination = _enemy.VideoDetected.transform.position;
                if (_enemy.HasAnimator) {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
                }
            }
        }
    }
}
