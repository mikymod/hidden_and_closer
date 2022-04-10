using UnityEngine;
using UnityEngine.AI;

namespace HNC
{
    public class NewEnemyIdleState : NewEnemyState
    {
        private Vector2 _randomPosition;
        private NavMeshPath path;
        private float _timer;
        public NewEnemyIdleState(NewEnemyController enemyController, EnemyFSMState state) : base(enemyController, state)
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
            var destination = _enemy.Patrol.NextPointInPath();
            _enemy.NavMeshAgent.destination = destination.position;
            _enemy.Animator.SetFloat("Speed", 1);
            _timer = Random.Range(_enemy.MinTimePatrol, _enemy.MaxTimePatrol);
        }

        public override void Exit()
        {
            _enemy.TransitionToIdleState = false;
        }

        public override void Update()
        {
            if (_enemy.NavMeshAgent.remainingDistance <= _enemy.NavMeshAgent.stoppingDistance)
            {
                //Stop Zombie
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
        }
    }
}
