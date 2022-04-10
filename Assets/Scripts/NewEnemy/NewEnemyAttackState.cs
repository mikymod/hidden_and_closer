namespace HNC
{
    public class NewEnemyAttackState : NewEnemyState
    {
        public NewEnemyAttackState(NewEnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter()
        {
            base.Enter();
            //CheckDistance();
        }

        public override void Exit() 
        {
            _enemy.TransitionToAttackState = false;
        }

        public override void Update()
        {
            _enemy.NavMeshAgent.SetDestination(_enemy.PosToGo);
            if (_enemy.DetectionSystem.visibleTargets.Count == 0)
            {
                _enemy.TransitionToSearchState = true;
            }
            //if (_enemy.HasAnimator && _enemy.NavMeshAgent.remainingDistance <= _enemy.AttackTreshoold)
            //{
            //    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
            //    _enemy.AnimatorComponent.SetTrigger(_enemy.AnimAttackHash);
            //    CheckHit();
            //}
            //CheckDistance();
        }

        private void CheckDistance()
        {
            //if ((_enemy.VideoDetected.transform.position - _enemy.transform.position).sqrMagnitude > _enemy.AttackTreshoold * _enemy.AttackTreshoold)
            //{
            //    _enemy.NavMeshAgent.destination = _enemy.VideoDetected.transform.position;
            //    if (_enemy.HasAnimator)
            //    {
            //        _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
            //    }
            //}
        }

        private void CheckHit()
        {
            //_enemy.IsFighting();
        }
    }
}
