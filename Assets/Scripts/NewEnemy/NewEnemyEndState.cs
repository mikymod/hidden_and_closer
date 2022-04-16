using UnityEngine;

namespace HNC {
    public class NewEnemyEndState : NewEnemyState {
        public NewEnemyEndState(NewEnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        private float _screamTimer;

        public override void Enter() {
            base.Enter();
            _screamTimer = Random.Range(_enemy.MinTimeScream, _enemy.MaxTimeScream);
            _enemy.Animator.SetFloat("Speed", 0);
            _enemy.NavMeshAgent.speed = 0;
            _enemy.NavMeshAgent.isStopped = true;
            if (_enemy.Target != null && Vector3.Distance(_enemy.Target.transform.position, _enemy.Target.transform.position) < _enemy.DistanceForHeating) {
                _enemy.Animator.SetTrigger("CarAttack");
            }
            _enemy.DetectionSystem.enabled = false;
        }

        public override void Exit() {

        }

        public override void Update() {
            _screamTimer -= Time.deltaTime;
            if (_screamTimer <= 0) {
                _enemy.Animator.SetTrigger("Scream");
                _screamTimer = Random.Range(_enemy.MinTimeScream, _enemy.MaxTimeScream);
            }
        }
    }
}
