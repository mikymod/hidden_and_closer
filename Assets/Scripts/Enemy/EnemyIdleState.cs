using UnityEngine;

namespace HNC {
    public class EnemyIdleState : EnemyState {
        //private enum ScreamState {
        //    Scream,
        //    Await,
        //}
        private float _timeToNextPoint;

        public EnemyIdleState(EnemyController enemyController, EnemyFSMState state) : base(enemyController, state) {
        }

        //private float _screamTimer = 0;
        //private ScreamState _screamState = ScreamState.Scream;


        public override void Enter() {
            base.Enter();
            _enemy.IdleGO.SetActive(true);
            GetRandomTarget();
        }

        private void GetRandomTarget() {
            //Caluclate random point
            Vector2 randomPoint = Random.onUnitSphere * _enemy.PatrolRadius;
            _enemy.NavMeshAgent.destination = _enemy.transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);
            if (_enemy.HasAnimator) {
                _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 1);
            }
        }

        public override void Exit() => _enemy.IdleGO.SetActive(false);

        public override void Update() {
            //_screamTimer -= Time.deltaTime;
            //if (_screamState == ScreamState.Scream) {
            //    if (_screamTimer <= 0) {
            //        _enemy.NavMeshAgent.velocity = Vector3.zero;
            //        _enemy.NavMeshAgent.angularSpeed = 0;
            //        if (_enemy.HasAnimator) {
            //            _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
            //            _enemy.AnimatorComponent.SetTrigger(_enemy.AnimScreamHash);
            //        }
            //        _screamTimer = Random.Range(_enemy.MinTimeScream, _enemy.MaxTimeScream);
            //        _screamState = ScreamState.Await;
            //    } else if (_enemy.NavMeshAgent.remainingDistance <= _enemy.PatrolTreshoold) {
            //        GetRandomTarget();
            //    }
            //} else {
            //    if (_screamTimer <= 0) {
            //        GetRandomTarget();
            //        _screamState = ScreamState.Scream;
            //        _screamTimer = Random.Range(_enemy.MinTimeScream, _enemy.MaxTimeScream);
            //    }
            //}
            if (_enemy.NavMeshAgent.remainingDistance <= _enemy.PatrolTreshoold) {
                if (_enemy.HasAnimator) {
                    _enemy.AnimatorComponent.SetFloat(_enemy.AnimSpeedHash, 0);
                }
                _timeToNextPoint = Random.Range(_enemy.MinTimePatrol, _enemy.MaxTimePatrol);
            }
            if (_timeToNextPoint > 0) {
                _timeToNextPoint -= Time.deltaTime;
                if (_timeToNextPoint <= 0) {
                    GetRandomTarget();
                }
            }
        }
    }
}
