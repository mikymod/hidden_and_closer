using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace HNC
{
    [Serializable]
    public enum EnemyFSMState
    {
        Idle,
        Alert,
        Search,
        Hunt,
        Attack,
        Death,
        End
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {

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
        [Tooltip("Time in Search State")]
        public float SearchTime;
        [Space(10)]
        [Tooltip("Distance from player to enemy for start Heating animation on player dead")]
        public float DistanceForHeating;
        [Tooltip("Min time range for scream after player dead")]
        public float MinTimeScream;
        [Tooltip("Max time range for scream after player dead")]
        public float MaxTimeScream;

        public DetectionSystem DetectionSystem;

        private StateMachine _stateMachine;
        public EnemyFSMState CurrentState;
        [HideInInspector] public NavMeshAgent NavMeshAgent;

        [HideInInspector] public float AlertTimer;
        [HideInInspector] public float SearchTimer;

        [HideInInspector] public bool TransitionToIdleState;
        [HideInInspector] public bool TransitionToAlertState;
        [HideInInspector] public bool TransitionToHuntState;
        [HideInInspector] public bool TransitionToAttackState;
        [HideInInspector] public bool TransitionToSearchState;
        [HideInInspector] public bool TransitionToEndState;

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

        public static UnityAction ForceIdleBroadcast;

        private void OnEnable()
        {
            DetectionSystem.NoiseDetected += CheckForNoisePosition;
            DetectionSystem.VisibleDetected += PlayerInLOS;
            DetectionSystem.ExitFromVisibleArea += PlayerNotInLOS;
            LightDetector.PlayerInLight += ScaleVisionArea;
            ForceIdleBroadcast += ForceIdle;
            PlayerController.DeadEvent += EnableTransitionToEndState;
        }


        private void OnDisable()
        {
            DetectionSystem.NoiseDetected -= CheckForNoisePosition;
            DetectionSystem.VisibleDetected -= PlayerInLOS;
            DetectionSystem.ExitFromVisibleArea -= PlayerNotInLOS;
            LightDetector.PlayerInLight -= ScaleVisionArea;
            ForceIdleBroadcast -= ForceIdle;
            PlayerController.DeadEvent -= EnableTransitionToEndState;
        }

        private void ForceIdle()
        {
            Patrol.enabled = false;
            DetectionSystem.enabled = false;
            TransitionToAlertState = false;
            TransitionToHuntState = false;
            TransitionToAttackState = false;
            TransitionToIdleState = true;
        }

        private void EnableTransitionToEndState()
        {
            TransitionToEndState = true;
        }

        private void Awake()
        {
            BodyCollider = GetComponent<CapsuleCollider>();
            Animator = GetComponent<Animator>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            NavMeshAgent.updatePosition = true;

            Patrol = GetComponent<Patrol>();

            _stateMachine = new StateMachine();
            EnemyIdleState idleState = new EnemyIdleState(this, EnemyFSMState.Idle);
            EnemyAlertState alertState = new EnemyAlertState(this, EnemyFSMState.Alert);
            EnemyHuntState huntState = new EnemyHuntState(this, EnemyFSMState.Hunt);
            EnemyAttackState attackState = new EnemyAttackState(this, EnemyFSMState.Attack);
            EnemySearchState searchState = new EnemySearchState(this, EnemyFSMState.Search);
            EnemyDeathState deathState = new EnemyDeathState(this, EnemyFSMState.Death);
            EnemyEndState endState = new EnemyEndState(this, EnemyFSMState.End);

            _stateMachine.AddTransition(idleState, alertState, () => TransitionToAlertState);
            _stateMachine.AddTransition(alertState, idleState, () => TransitionToIdleState);
            _stateMachine.AddTransition(alertState, huntState, () => TransitionToHuntState);
            _stateMachine.AddTransition(huntState, attackState, () => TransitionToAttackState);
            _stateMachine.AddTransition(attackState, huntState, () => TransitionToHuntState);
            _stateMachine.AddTransition(huntState, searchState, () => TransitionToSearchState);
            _stateMachine.AddTransition(searchState, idleState, () => TransitionToIdleState);
            _stateMachine.AddTransition(searchState, huntState, () => TransitionToHuntState);
            _stateMachine.AddAnyTransition(deathState, () => life <= 0);
            _stateMachine.AddAnyTransition(endState, () => TransitionToEndState);
            _stateMachine.SetInitialState(idleState);
        }

        private void Update()
        {
            _stateMachine.Update();
            if (Target != null)
            {
                PosToGo = Target.position;
            }
        }

        public void CheckForNoisePosition(Vector3 pos)
        {
            if (Target != null)
            {
                return;
            }
            if (CurrentState == EnemyFSMState.Idle)
            {
                TransitionToAlertState = true;
            }
            PosToGo = pos;
        }

        private void PlayerInLOS(Transform target)
        {
            PosToGo = target.position;
            Target = target;
            if (CurrentState == EnemyFSMState.Idle)
            {
                TransitionToAlertState = true;
                return;
            }
            if (CurrentState == EnemyFSMState.Alert || CurrentState == EnemyFSMState.Search)
            {
                TransitionToHuntState = true;
                return;
            }
        }

        private void PlayerNotInLOS()
        {
            if (CurrentState == EnemyFSMState.Attack)
            {
                TransitionToHuntState = true;
            }

            PosToGo = Target.position;
            Target = null;
        }

        public void Damaged() => life = 0;

        private void ScaleVisionArea(bool isPlayerInLight) => DetectionSystem.viewRadius = isPlayerInLight ? 8 : 4;

        public void BackFromAttackState()
        {
            StartCoroutine(BackFromAttackStateCoroutine());
        }

        private IEnumerator BackFromAttackStateCoroutine()
        {
            yield return new WaitForSeconds(1.3f);

            TransitionToAttackState = false;
            TransitionToHuntState = true;
        }
    }
}
