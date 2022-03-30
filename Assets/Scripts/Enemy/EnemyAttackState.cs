namespace HNC {
    public class EnemyAttackState : IState {
        private readonly EnemyController _enemy;

        public EnemyAttackState(EnemyController enemy) => _enemy = enemy;

        public void Enter() {
            //_enemy.AttackGO.SetActive(true);
            _enemy.debugTest.text = "Attack State";
            _enemy.NavMeshAgent.SetDestination(_enemy.detected.transform.position);
        }

        public void Exit() { }

        public void Update() {
            _enemy.NavMeshAgent.SetDestination(_enemy.detected.transform.position);
            if (_enemy.HasAnimator && _enemy.NavMeshAgent.remainingDistance <= _enemy.AttackTreshoold) {
                _enemy.AnimatorComponent.SetTrigger(_enemy.AnimAttackHash);
            }
        }
    }
}
