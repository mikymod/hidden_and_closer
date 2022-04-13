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

        public DetectionSystem DetectionSystem;

        private StateMachine _stateMachine;
        [HideInInspector] public EnemyFSMState CurrentState;
        [HideInInspector] public NavMeshAgent NavMeshAgent;

        [HideInInspector] public float AlertTimer;
        [HideInInspector] public float SearchTimer;

        [HideInInspector] public bool TransitionToIdleState;
        [HideInInspector] public bool TransitionToAlertState;
        [HideInInspector] public bool TransitionToAttackState;
        [HideInInspector] public bool TransitionToSearchState;

        [Header("hearing")]
        public float hearingRadius;
        public LayerMask soundMask;

        [HideInInspector] public Vector3 PosToGo;
        [HideInInspector] public Transform Target;

        private int life = 1;

        [HideInInspector] public Animator Animator;
        [HideInInspector] public CapsuleCollider BodyCollider;
        [HideInInspector] public CapsuleCollider ArmCollider;

        [HideInInspector] public Patrol Patrol;

        public ZombieUI UI;

        private void OnEnable() {
            DetectionSystem.NoiseDetected += CheckForNoisePosition;
            DetectionSystem.VisibleDetected += PlayerInLOS;
            DetectionSystem.ExitFromVisibleArea += PlayerNotInLOS;
            LightDetector.PlayerInLight += ScaleVisionArea;
            PlayerController.DeadEvent += ForceIdle;
        }


        private void OnDisable() {
            DetectionSystem.NoiseDetected -= CheckForNoisePosition;
            DetectionSystem.VisibleDetected -= PlayerInLOS;
            DetectionSystem.ExitFromVisibleArea -= PlayerNotInLOS;
            LightDetector.PlayerInLight -= ScaleVisionArea;
            PlayerController.DeadEvent -= ForceIdle;
        }

        private void ForceIdle()
        {
            // NavMeshAgent.enabled = false;
            Patrol.enabled = false;
            DetectionSystem.enabled = false;

            TransitionToAlertState = false;
            TransitionToAttackState = false;
            TransitionToIdleState = true;

        }

        private void Awake() {
            BodyCollider = GetComponent<CapsuleCollider>();
            Animator = GetComponent<Animator>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            NavMeshAgent.updatePosition = true;

            Patrol = GetComponent<Patrol>();

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

        private void Update() {
            _stateMachine.Update();
            if (Target != null) {
                PosToGo = Target.position;
            }
        }

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

        private void ScaleVisionArea(bool isPlayerInLight) => DetectionSystem.viewRadius = isPlayerInLight ? 8 : 4;
    }
}
