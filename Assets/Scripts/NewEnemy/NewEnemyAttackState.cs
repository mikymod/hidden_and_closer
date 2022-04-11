using UnityEngine;

namespace HNC {
    public class NewEnemyAttackState : NewEnemyState {
        private float _timeBetweenAttack;

        public NewEnemyAttackState(NewEnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter()
        {
            base.Enter();//CheckDistance();
            _enemy.Animator.SetFloat("Speed", 2);
            _enemy.NavMeshAgent.speed *= 2;
        }

        public override void Exit()
        {
            _enemy.TransitionToAttackState = false;
            _enemy.NavMeshAgent.speed /= 2;
        } 

        public override void Update() {
            _enemy.NavMeshAgent.SetDestination(_enemy.PosToGo);
            if (_enemy.DetectionSystem.visibleTargets.Count == 0) {
                _enemy.TransitionToSearchState = true;
            }
            if (_enemy.NavMeshAgent.remainingDistance < _enemy.NavMeshAgent.stoppingDistance)
            {
                //if (_enemy.NavMeshAgent.remainingDistance < 2) {
                _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                _enemy.Animator.SetFloat("Speed", 0);
                _timeBetweenAttack -= Time.deltaTime;
                if (_timeBetweenAttack <= 0) {

                    _enemy.Animator.SetTrigger("Attack");
                    //if (_enemy.Target.gameObject.layer == LayerMask.NameToLayer("Player"))
                    //{
                    //    _enemy.Animator.SetTrigger("Attack");
                    //    CompanionController.OnCompanionDestroy?.Invoke();
                    //}
                    //else if (_enemy.Target.gameObject.layer == LayerMask.NameToLayer("Companion"))
                    //{
                    //    _enemy.Animator.SetTrigger("CarAttack");
                    //}
                }
            } else {
                _timeBetweenAttack = _enemy.TimeBetweenAttack;
            }
        }
    }
}
