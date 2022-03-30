namespace HNC
{
    public class EnemyAttackState : IState
    {
        private readonly EnemyController _enemy;

        public EnemyAttackState(EnemyController enemy) => _enemy = enemy;

        public void Enter()
        {
            _enemy.AttackGO.SetActive(true);
            CheckDistance();
        }

        public void Exit() { }

        public void Update()
        {
            if (_enemy.HasAnimator && _enemy.NavMeshAgent.remainingDistance <= _enemy.AttackTreshoold)
            {
                _enemy.AnimatorComponent.SetTrigger(_enemy.AnimAttackHash);
            }
            CheckDistance();
        }

        private void CheckDistance()
        {
            if ((_enemy.VideoDetected.transform.position - _enemy.transform.position).sqrMagnitude > _enemy.AlertTreshoold * _enemy.AlertTreshoold)
            {
                _enemy.NavMeshAgent.destination = _enemy.VideoDetected.transform.position;
                if (_enemy.HasAnimator)
                {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
                }
            }
        }
    }
}
