using HNC.Audio;
using UnityEngine;
using UnityEngine.AI;

namespace HNC
{
    public class EnemySearchState : EnemyState
    {

        private Vector2 _randomPosition;
        private NavMeshPath path;
        private float _timer;
        public EnemySearchState(EnemyController enemy, EnemyFSMState state) : base(enemy, state)
        {
            path = new NavMeshPath();
        }


        public override void Enter()
        {
            base.Enter();
            _enemy.NavMeshAgent.SetDestination(_enemy.PosToGo);
            _enemy.NavMeshAgent.isStopped = false;
            _enemy.Animator.SetFloat("Speed", 1);
            _enemy.SearchTimer = _enemy.SearchTime;
        }

        public override void Update()
        {
            _enemy.SearchTimer -= Time.deltaTime;
            if (_enemy.SearchTimer <= 0)
            {
                _enemy.TransitionToIdleState = true;
            }
            if (_enemy.NavMeshAgent.remainingDistance <= _enemy.NavMeshAgent.stoppingDistance)
            {
                _enemy.NavMeshAgent.isStopped = true;
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

        private void GetRandomTarget()
        {
            if (_enemy.TryGetComponent(out AudioZombieController component))
            {
                component.PlaySearchSound();
            }
            _enemy.Animator.SetFloat("Speed", 1);
            _enemy.NavMeshAgent.isStopped = false;
            _timer = Random.Range(_enemy.MinTimePatrol / 2, _enemy.MaxTimePatrol / 2);
            do
            {
                _randomPosition = Random.insideUnitCircle * _enemy.PatrolRadius * 2;
            } while (!_enemy.NavMeshAgent.CalculatePath(_enemy.transform.position + new Vector3(
                Mathf.Clamp(_randomPosition.x, 1, _enemy.PatrolRadius), 0, Mathf.Clamp(_randomPosition.y, 1, _enemy.PatrolRadius)), path));
            _enemy.NavMeshAgent.path = path;

        }

        public override void Exit()
        {
            _enemy.TransitionToSearchState = false;
        }
    }
}