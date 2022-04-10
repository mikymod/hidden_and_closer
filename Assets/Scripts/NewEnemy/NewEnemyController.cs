using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HNC {
    [RequireComponent(typeof(NavMeshAgent))]
    public class NewEnemyController : MonoBehaviour {
        [Header("StateMachine")]
        [Tooltip("Min time range for check next point in Patrol")]
        public float MinTimePatrol;
        [Tooltip("Max time range for check next point in Patrol")]
        public float MaxTimePatrol;
        [Tooltip("Radius for choose a Patrol target")]
        public float PatrolRadius;
        [Space(10)]
        [Tooltip("Time in Alert State")]
        public float AlertTime;
        [Space(10)]
        [Tooltip("Time between attack")]
        public float TimeBetweenAttack;
        [Space(10)]
        [Tooltip("Time in Search State")]
        public float SearchTime;

        [Header("NavMesh")]
        [Tooltip("Treshoold of patrol")]
        public float PatrolTreshoold;
        [Tooltip("Treshoold of alert")]
        public float AlertTreshoold;
        [Tooltip("Treshoold of attack")]
        public float AttackTreshoold;

        public DetectionSystem DetectionSystem;

        private StateMachine _stateMachine;
        [HideInInspector] public EnemyFSMState CurrentState;
        [HideInInspector] public NavMeshAgent NavMeshAgent;

        [HideInInspector] public float AlertTimer;
        [HideInInspector] public float SearchTimer;

        public bool TransitionToIdleState;
        public bool TransitionToAlertState;
        public bool TransitionToAttackState;
        public bool TransitionToSearchState;

        [Header("hearing")]
        public float hearingRadius;
        public LayerMask soundMask;

        public Vector3 PosToGo;
        public Transform Target;

        public List<Vector3> SoundsToCheck = new List<Vector3>();
        private int life = 1;

        public Animator Animator;
        public CapsuleCollider Collider;

        private void OnEnable() {
            DetectionSystem.NoiseDetected += CheckForNoisePosition;
            DetectionSystem.VisibleDetected += PlayerInLOS;
            DetectionSystem.ExitFromVisibleArea += PlayerNotInLOS;
        }

        private void OnDisable() {
            DetectionSystem.NoiseDetected -= CheckForNoisePosition;
            DetectionSystem.VisibleDetected -= PlayerInLOS;
            DetectionSystem.ExitFromVisibleArea -= PlayerNotInLOS;
        }

        private void Awake() {
            Collider = GetComponent<CapsuleCollider>();
            Animator = GetComponent<Animator>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            NavMeshAgent.updatePosition = true;

            _stateMachine = new StateMachine();
            NewEnemyIdleState idleState = new NewEnemyIdleState(this, EnemyFSMState.Idle);
            NewEnemyAlertState alertState = new NewEnemyAlertState(this, EnemyFSMState.Alert);
            NewEnemyAttackState attackState = new NewEnemyAttackState(this, EnemyFSMState.Attack);
            NewEnemySearchState searchState = new NewEnemySearchState(this, EnemyFSMState.Search);
            NewEnemyDeathState deathState = new NewEnemyDeathState(this, EnemyFSMState.Death);

            _stateMachine.AddTransition(idleState, alertState, () => TransitionToAlertState);
            _stateMachine.AddTransition(alertState, idleState, () => TransitionToIdleState);
            _stateMachine.AddTransition(alertState, attackState, () => TransitionToAttackState);
            _stateMachine.AddTransition(attackState, searchState, () => TransitionToSearchState);
            _stateMachine.AddTransition(searchState, idleState, () => TransitionToIdleState);
            _stateMachine.AddTransition(searchState, attackState, () => TransitionToAttackState);
            _stateMachine.AddAnyTransition(deathState, () => life <= 0);
            _stateMachine.SetInitialState(idleState);
        }

        private void Update() => _stateMachine.Update();//Debug.Log(CurrentState);

        public void CheckForNoisePosition(Vector3 pos) {
            if (Target != null) {
                return;
            }
            TransitionToAlertState = true;
            PosToGo = pos;
        }

        private void PlayerInLOS(Transform target) {
            TransitionToAlertState = true;
            TransitionToAttackState = true;
            PosToGo = target.position;
            Target = target;
        }

        private void PlayerNotInLOS() {
            PosToGo = Target.position;
            Target = null;
        }

        public void Damaged() => life = 0;

        private IEnumerator AttackRoutine(float time) {
            yield return new WaitForSeconds(time);
            if (Physics.CheckCapsule(transform.position, transform.forward, 1)) {
                PlayerController.DeadEvent?.Invoke();
            }
        }
        public void Fight() => StartCoroutine(AttackRoutine(1.5f));
    }
}
