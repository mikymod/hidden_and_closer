using UnityEngine;

namespace HNC
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(EnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter()
        {
            base.Enter();
            _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
            _enemy.Animator.SetFloat("Speed", 0);
            _enemy.NavMeshAgent.isStopped = true;

            if (_enemy.Target != null)
            {
                if (_enemy.Target.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    _enemy.Animator.SetTrigger("Attack");
                }
                else if (_enemy.Target.gameObject.layer == LayerMask.NameToLayer("Companion"))
                {
                    _enemy.Animator.SetTrigger("CarAttack");
                    CompanionController.OnCompanionDestroy?.Invoke();
                }

                _enemy.BackFromAttackState();
            }
        }

        public override void Exit()
        {
            _enemy.TransitionToAttackState = false;
        }

        public override void Update()
        {
            if (_enemy.Target != null)
            {
                _enemy.transform.LookAt(_enemy.Target.position, Vector3.up);
                _enemy.transform.rotation = Quaternion.Euler(0, _enemy.transform.rotation.eulerAngles.y, 0);
            }
        }
    }
}
