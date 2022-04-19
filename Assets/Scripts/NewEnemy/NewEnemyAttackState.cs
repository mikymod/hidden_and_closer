using UnityEngine;

namespace HNC
{
    public class NewEnemyAttackState : NewEnemyState
    {
        private float _timeBetweenAttack;

        public NewEnemyAttackState(NewEnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter()
        {
            base.Enter();//CheckDistance();
            _enemy.NavMeshAgent.isStopped = false;
            _enemy.Animator.SetFloat("Speed", 2);
            //_enemy.NavMeshAgent.speed = _enemy.SpeedAttack;
        }

        public override void Exit()
        {
            //_enemy.NavMeshAgent.speed = _enemy.SpeedNormal;
            _enemy.TransitionToAttackState = false;
        }

        public override void Update()
        {
            _enemy.NavMeshAgent.SetDestination(_enemy.PosToGo);
            if (_enemy.DetectionSystem.visibleTargets.Count == 0)
            {
                _enemy.TransitionToSearchState = true;
            }
            if (_enemy.NavMeshAgent.remainingDistance < _enemy.NavMeshAgent.stoppingDistance)
            {
                //if (_enemy.NavMeshAgent.remainingDistance < 2) {
                _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                _enemy.Animator.SetFloat("Speed", 0);
                _enemy.NavMeshAgent.isStopped = true;
                _timeBetweenAttack -= Time.deltaTime;
                if (_timeBetweenAttack <= 0)
                {

                    //_enemy.Animator.SetTrigger("Attack");
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
                    }
                    else
                    {
                        Debug.Log("Non ho pi� niente", _enemy.gameObject);
                    }
                }
            }
            else
            {
                _timeBetweenAttack = _enemy.TimeBetweenAttack;
                _enemy.Animator.SetFloat("Speed", 2);
                _enemy.NavMeshAgent.isStopped = false;
            }
            if (_enemy.Target != null)
            {
                _enemy.transform.LookAt(_enemy.Target.position, Vector3.up);
                _enemy.transform.rotation = Quaternion.Euler(0, _enemy.transform.rotation.eulerAngles.y, 0);
            }
        }
    }
}
