using UnityEngine;
using UnityEngine.AI;

namespace HNC
{
    public class EnemyIdleState : EnemyState
    {
        private float _timer;
        private NavMeshPath path;

        public EnemyIdleState(EnemyController enemyController, EnemyFSMState state) : base(enemyController, state)
        {
            path = new NavMeshPath();
        }

        public override void Enter()
        {
            base.Enter();
            MoveToNextPoint();
        }

        private void MoveToNextPoint()
        {
            _timer = Random.Range(_enemy.MinTimePatrol, _enemy.MaxTimePatrol);
            var destination = _enemy.Patrol.NextPointInPath();
            _enemy.NavMeshAgent.destination = destination.position;
            _enemy.NavMeshAgent.isStopped = false;
            _enemy.Animator.SetFloat("Speed", 1);
        }

        public override void Exit()
        {
            _enemy.TransitionToIdleState = false;
        }

        public override void Update()
        {
            if (_enemy.NavMeshAgent.remainingDistance <= _enemy.NavMeshAgent.stoppingDistance)
            {
                _enemy.NavMeshAgent.isStopped = true;
                _enemy.Animator.SetFloat("Speed", 0);

                if (_timer <= 0)
                {
                    MoveToNextPoint();
                }
                else
                {
                    _timer -= Time.deltaTime;
                }
            }
            else
            {
                _enemy.Animator.SetFloat("Speed", 1);
                _enemy.NavMeshAgent.isStopped = false;
            }
        }
    }
}
