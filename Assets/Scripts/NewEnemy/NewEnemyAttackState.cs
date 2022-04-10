using UnityEngine;

namespace HNC {
    public class NewEnemyAttackState : NewEnemyState {
        private float _timeBetweenAttack;

        public NewEnemyAttackState(NewEnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter() => base.Enter();//CheckDistance();

        public override void Exit() => _enemy.TransitionToAttackState = false;

        public override void Update() {
            _enemy.NavMeshAgent.SetDestination(_enemy.PosToGo);
            if (_enemy.DetectionSystem.visibleTargets.Count == 0) {
                _enemy.TransitionToSearchState = true;
            }
            //if(_enemy.NavMeshAgent.remainingDistance < _enemy.NavMeshAgent.stoppingDistance) {
            if (_enemy.NavMeshAgent.remainingDistance < 2) {
                _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                _enemy.Animator.SetFloat("Speed", 0);
                _timeBetweenAttack -= Time.deltaTime;
                if (_timeBetweenAttack <= 0) {
                    _enemy.Animator.SetTrigger("Attack");
                    _enemy.Fight();
                }
            } else {
                _timeBetweenAttack = _enemy.TimeBetweenAttack;
            }
        }
    }
}
