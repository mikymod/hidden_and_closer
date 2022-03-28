using UnityEngine;
using UnityEngine.AI;

namespace HNC {
    public class EnemyIdleState : IState {
        private enum ScreamState {
            Scream,
            Await,
        }
        private readonly EnemyController _enemy;
        private float _screamTimer = 0;
        private ScreamState _screamState = ScreamState.Scream;

        public EnemyIdleState(EnemyController enemy) => _enemy = enemy;

        public void Enter() {
            GetRandomTarget();
            _screamTimer = Random.Range(_enemy.MinTimeScream, _enemy.MaxTimeScream);
        }

        private void GetRandomTarget() {
            //Caluclate random point
            Vector2 randomPoint = Random.insideUnitSphere * _enemy.PatrolRadius;
            _enemy.NavMeshAgent.SetDestination(_enemy.transform.position + new Vector3(randomPoint.x, 0, randomPoint.y));
            _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
        }

        public void Exit() {
        }

        public void Update() {
            _screamTimer -= Time.deltaTime;
            if (_screamState == ScreamState.Scream) {
                if (_screamTimer <= 0) {
                    _enemy.NavMeshAgent.velocity = Vector3.zero;
                    _enemy.NavMeshAgent.angularSpeed = 0;
                    if (_enemy.HasAnimator) {
                        _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
                        _enemy.AnimatorComponent.SetTrigger(_enemy.AnimScreamHash);
                    }
                    _screamTimer = Random.Range(_enemy.MinTimeScream, _enemy.MaxTimeScream);
                    _screamState = ScreamState.Await;
                } else if (_enemy.NavMeshAgent.remainingDistance <= _enemy.PatrolTreshoold) {
                    GetRandomTarget();
                }
            } else {
                if (_screamTimer <= 0) {
                    GetRandomTarget();
                    _screamState = ScreamState.Scream;
                    _screamTimer = Random.Range(_enemy.MinTimeScream, _enemy.MaxTimeScream);
                }
            }
        }
    }
}
