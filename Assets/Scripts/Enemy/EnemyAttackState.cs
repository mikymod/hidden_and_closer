using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC {
    public class EnemyAttackState : IState {
        private EnemyController _enemy;

        public EnemyAttackState(EnemyController enemy) {
            _enemy = enemy;
        }

        public void Enter() {
            _enemy.NavMeshAgent.SetDestination(_enemy.detected.transform.position);
        }

        public void Exit() {            
        }

        public void Update() {
            _enemy.NavMeshAgent.SetDestination(_enemy.detected.transform.position);
            if(_enemy.HasAnimator && _enemy.NavMeshAgent.remainingDistance <= _enemy.AttackTreshoold) {
                _enemy.AnimatorComponent.SetTrigger(_enemy.AnimAttackHash);
            }
        }
    }
}
