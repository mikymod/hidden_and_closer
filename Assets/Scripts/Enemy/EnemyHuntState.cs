using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class EnemyHuntState : EnemyState
    {
        public EnemyHuntState(EnemyController enemy, EnemyFSMState state) : base(enemy, state)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _enemy.NavMeshAgent.isStopped = false;
            _enemy.Animator.SetFloat("Speed", 2);
        }

        public override void Exit()
        {
            _enemy.TransitionToHuntState = false;
        }

        public override void Update()
        {
            _enemy.NavMeshAgent.SetDestination(_enemy.PosToGo);
            if (_enemy.Target != null)
            {
                _enemy.transform.LookAt(_enemy.Target.position, Vector3.up);
                _enemy.transform.rotation = Quaternion.Euler(0, _enemy.transform.rotation.eulerAngles.y, 0);
            }

            if (_enemy.DetectionSystem.visibleTargets.Count == 0)
            {
                _enemy.TransitionToSearchState = true;
                return;
            }

            if (_enemy.NavMeshAgent.remainingDistance < _enemy.NavMeshAgent.stoppingDistance * 2)
            {
                _enemy.TransitionToHuntState = false;
                _enemy.TransitionToAttackState = true;
            }

        }
    }
}
