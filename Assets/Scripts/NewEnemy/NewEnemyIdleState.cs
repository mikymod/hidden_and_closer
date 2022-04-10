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
            GetRandomTarget();
        }

        private void GetRandomTarget()
        {
            _enemy.Animator.SetFloat("Speed", 1);
            //Caluclate random point
            _timer = Random.Range(_enemy.MinTimePatrol, _enemy.MaxTimePatrol);
            do
            {
                _randomPosition = Random.insideUnitCircle * _enemy.PatrolRadius;
            } while (!_enemy.NavMeshAgent.CalculatePath(_enemy.transform.position + new Vector3(
                Mathf.Clamp(_randomPosition.x, 1, _enemy.PatrolRadius), 0, Mathf.Clamp(_randomPosition.y, 1, _enemy.PatrolRadius)), path));
            _enemy.NavMeshAgent.path = path;
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
                    GetRandomTarget();
                }
                else
                {
                    _timer -= Time.deltaTime;
                }
            }
        }
    }
}
