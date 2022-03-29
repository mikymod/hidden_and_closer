using UnityEngine;
using UnityEngine.AI;

namespace HNC {
    [RequireComponent(typeof(Detector))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour {
        [Header("Debug")]
        public GameObject IdleGO;
        public GameObject SuspGO;
        public GameObject AlertGO;
        public GameObject AttackGO;
        public GameObject SearchGO;
        public GameObject DetectedGO;
        //public bool PullAvatarTowardsAgent;
        //public bool PullAgentTowardsAvatar;

        [Header("Character")]
        public float MovementSpeed;
        public float RotationSpeed;

        [Header("StateMachine")]
        [Tooltip("Min time range for random animation of scream")]
        public float MinTimeScream;
        [Tooltip("Max time range for random animation of scream")]
        public float MaxTimeScream;
        [Tooltip("Radius for choose a Patrol target")]
        public float PatrolRadius;
        [Tooltip("Treshoold of patrol")]
        public float PatrolTreshoold;
        [Space(10)]
        [Tooltip("Time in Suspicious State")]
        public float SuspiciousTime;
        [Space(10)]
        [Tooltip("Treshoold of attack")]
        public float AttackTreshoold;
        [Tooltip("Time in Alert State")]
        public float AlertTime;
        [Space(10)]
        [Tooltip("Time in Search State")]
        public float SearchTime;

        [Header("Animator")]
        [Tooltip("Speed float name")]
        [SerializeField] private string _speedParamName;
        [Tooltip("Scream trigger name")]
        [SerializeField] private string _screamParamName;
        [Tooltip("Attack trigger name")]
        [SerializeField] private string _attackParamName;

        //State Machine
        private StateMachine _stateMachine;
        private readonly float life = 20;
        [HideInInspector] public NavMeshAgent NavMeshAgent;
        [HideInInspector] public Quaternion Rotation;

        [HideInInspector] public Animator AnimatorComponent;
        public bool HasAnimator => AnimatorComponent != null;
        [HideInInspector] public int AnimSpeedHash;
        [HideInInspector] public int AnimScreamHash;
        [HideInInspector] public int AnimAttackHash;


        //Detection
        public GameObject detected { get; private set; }
        [HideInInspector] public float SuspiciousTimer;
        [HideInInspector] public float AlertTimer;
        [HideInInspector] public float SearchTimer;

        private void Awake() {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            NavMeshAgent.updatePosition = false;

            AnimatorComponent = GetComponent<Animator>();
            AnimSpeedHash = Animator.StringToHash(_speedParamName);
            AnimScreamHash = Animator.StringToHash(_screamParamName);
            AnimAttackHash = Animator.StringToHash(_attackParamName);

            SuspiciousTimer = SuspiciousTime + 1;
            AlertTimer = AlertTime + 1;
            SearchTimer = SearchTime + 1;

            _stateMachine = new StateMachine();
            EnemyIdleState idleState = new EnemyIdleState(this);
            EnemySuspiciousState suspiciousState = new EnemySuspiciousState(this);
            EnemyAlertState alertState = new EnemyAlertState(this);
            EnemyAttackState attackState = new EnemyAttackState(this);
            EnemySearchState searchState = new EnemySearchState(this);
            EnemyDeathState deathState = new EnemyDeathState(this);

            _stateMachine.AddAnyTransition(deathState, () => life <= 0);

            _stateMachine.AddTransition(idleState, suspiciousState, () => { /*Debug.Log($"Idle to Susp {detected != null}");*/ return detected != null; });
            _stateMachine.AddTransition(suspiciousState, alertState, () => { /*Debug.Log($"Susp to Alert {SuspiciousTimer <= 0 && detected != null}");*/ return SuspiciousTimer <= 0 && detected != null; });
            _stateMachine.AddTransition(suspiciousState, searchState, () => { /*Debug.Log($"Susp to Search {SuspiciousTimer <= 0 && detected == null}");*/ return SuspiciousTimer <= 0 && detected == null; });
            _stateMachine.AddTransition(alertState, attackState, () => { /*Debug.Log($"Alert to Attack {AlertTimer <= 0 && detected != null}");*/ return AlertTimer <= 0 && detected != null; });
            _stateMachine.AddTransition(alertState, searchState, () => { /*Debug.Log($"Alert to Search {AlertTimer <= 0 && detected == null}");*/ return AlertTimer <= 0 && detected == null; });
            _stateMachine.AddTransition(attackState, searchState, () => { /*Debug.Log($"Attack to Search {detected == null}");*/ return detected == null; });
            _stateMachine.AddTransition(searchState, idleState, () => { /*Debug.Log($"Search to Idle {SearchTimer <= 0}");*/ return SearchTimer <= 0; });
            _stateMachine.AddTransition(searchState, suspiciousState, () => { /*Debug.Log($"Search to Idle {SearchTimer <= 0}");*/ return detected != null; });

            _stateMachine.SetInitialState(idleState);
        }

        private void OnEnable() {
            DetectionSystemEvents.OnAudioDetectEnter += OnDetectionEnter;
            DetectionSystemEvents.OnVisionDetectEnter += OnDetectionEnter;
            DetectionSystemEvents.OnAudioDetectExit += OnDetectionExit;
            DetectionSystemEvents.OnVisionDetectExit += OnDetectionExit;
        }

        private void OnDisable() {
            DetectionSystemEvents.OnAudioDetectEnter -= OnDetectionEnter;
            DetectionSystemEvents.OnVisionDetectEnter -= OnDetectionEnter;
            DetectionSystemEvents.OnAudioDetectExit -= OnDetectionExit;
            DetectionSystemEvents.OnVisionDetectExit -= OnDetectionExit;
        }

        private void OnAnimatorMove() {
            Vector3 position = AnimatorComponent.rootPosition;
            position.y = NavMeshAgent.nextPosition.y;
            transform.position = position;

            if (Vector3.Distance(transform.position, NavMeshAgent.nextPosition) > NavMeshAgent.radius) {
                //if (PullAvatarTowardsAgent) {
                //    transform.position += (NavMeshAgent.nextPosition - transform.position) * 0.1f;
                //} else if (PullAgentTowardsAvatar) {
                NavMeshAgent.nextPosition += (transform.position - NavMeshAgent.nextPosition) * 0.1f;
                //}
            }
        }

        private void OnDetectionEnter(GameObject detecter, GameObject detected) {
            //Ho rilevato io
            if (detecter == gameObject) {
                //Non ho un target
                //OPPURE
                //Lo considero solo se sono in Susp
                if (this.detected == null || SuspiciousTimer >= SuspiciousTime && SuspiciousTimer < 0) {
                    //Setto detected e target
                    this.detected = detected;
                    DetectedGO.SetActive(true);

                    //Attivo il suspTimer
                    SuspiciousTimer = SuspiciousTime;
                }
            }
        }

        private void OnDetectionExit(GameObject detecter, GameObject detected) {
            //Ho rilevato io e stavo già rilevando detected
            if (detecter == gameObject && this.detected != null && this.detected == detected) {
                //Rimuovo il detected
                this.detected = null;
                DetectedGO.SetActive(false);

                //Attivo il searchTimer
                SearchTimer = SearchTime;
            }

        }

        private void Update() => _stateMachine.Update();//transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, RotationSpeed * Time.deltaTime);//transform.position = Vector3.Lerp(transform.position, Target, MovementSpeed * Time.deltaTime);

    }
}
