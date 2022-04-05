using UnityEngine;
using UnityEngine.AI;
using TMPro;

namespace HNC
{
    public enum EnemyFSMState {
        Idle,
        Suspicious,
        Alert,
        Attack,
        Search,
        Death
    }

    [RequireComponent(typeof(Detector))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Debug")]
        public GameObject DetectedVideoGO;
        public GameObject DetectedAudioGO;
        //public bool PullAvatarTowardsAgent;
        //public bool PullAgentTowardsAvatar;

        [Header("Character")]
        public float MovementSpeed;
        public float RotationSpeed;

        [Header("StateMachine")]
        [Tooltip("Min time range for check next point in Patrol")]
        public float MinTimePatrol;
        [Tooltip("Max time range for check next point in Patrol")]
        public float MaxTimePatrol;
        [Tooltip("Radius for choose a Patrol target")]
        public float PatrolRadius;
        [Space(10)]
        [Tooltip("Time in Suspicious State")]
        public float SuspiciousTime;
        [Space(10)]
        [Tooltip("Time in Alert State")]
        public float AlertTime;

        [Header("NavMesh")]
        [Tooltip("Treshoold of patrol")]
        public float PatrolTreshoold;
        [Tooltip("Treshoold of suspicious")]
        public float SuspiciousTreshoold;
        [Tooltip("Treshoold of alert")]
        public float AlertTreshoold;
        [Tooltip("Treshoold of attack")]
        public float AttackTreshoold;

        [Header("Animator")]
        [Tooltip("Speed float name")]
        [SerializeField] private string _speedParamName;
        [Tooltip("Scream trigger name")]
        [SerializeField] private string _screamParamName;
        [Tooltip("Attack trigger name")]
        [SerializeField] private string _attackParamName;
        [Tooltip("Death float name")]
        [SerializeField] private string _deathParamName;

        public ZombieUI UI;

        //State Machine
        private StateMachine _stateMachine;
        [HideInInspector] public EnemyFSMState CurrentState;
        private float life = 20;
        [HideInInspector] public NavMeshAgent NavMeshAgent;
        [HideInInspector] public Quaternion Rotation;

        [HideInInspector] public Animator AnimatorComponent;
        public bool HasAnimator => AnimatorComponent != null;
        [HideInInspector] public int AnimSpeedHash;
        [HideInInspector] public int AnimScreamHash;
        [HideInInspector] public int AnimAttackHash;
        [HideInInspector] public int AnimDeathHash;


        //Detection
        public GameObject Detected { get; private set; }
        public GameObject VideoDetected { get; private set; }
        public GameObject AudioDetected { get; private set; }
        [HideInInspector] public float SuspiciousTimer;
        [HideInInspector] public float AlertTimer;
        //[HideInInspector] public float SearchTimer;

        EnemyDeathState deathState;
        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            NavMeshAgent.updatePosition = false;

            AnimatorComponent = GetComponent<Animator>();
            AnimSpeedHash = Animator.StringToHash(_speedParamName);
            AnimScreamHash = Animator.StringToHash(_screamParamName);
            AnimAttackHash = Animator.StringToHash(_attackParamName);
            AnimDeathHash = Animator.StringToHash(_deathParamName);

            SuspiciousTimer = SuspiciousTime + 1;
            AlertTimer = AlertTime + 1;

            _stateMachine = new StateMachine();
            EnemyIdleState idleState = new EnemyIdleState(this, EnemyFSMState.Idle);
            EnemySuspiciousState suspiciousState = new EnemySuspiciousState(this, EnemyFSMState.Suspicious);
            EnemyAlertState alertState = new EnemyAlertState(this, EnemyFSMState.Alert);
            EnemyAttackState attackState = new EnemyAttackState(this, EnemyFSMState.Attack);
            EnemySearchState searchState = new EnemySearchState(this, EnemyFSMState.Search);
            deathState = new EnemyDeathState(this, EnemyFSMState.Death);
            _stateMachine.AddAnyTransition(deathState, () => life <= 0);

            _stateMachine.AddTransition(idleState, suspiciousState, () =>
            {
                /*Debug.Log($"Idle to Susp {detected != null}");*/
                if (VideoDetected != null)
                {
                    Detected = VideoDetected;
                }
                else if (AudioDetected != null)
                {
                    Detected = AudioDetected;
                }
                return VideoDetected != null || AudioDetected != null;
            });
            _stateMachine.AddTransition(suspiciousState, alertState, () =>
            {
                /*Debug.Log($"Susp to Alert {SuspiciousTimer <= 0 && detected != null}");*/
                return SuspiciousTimer <= 0 && VideoDetected != null;
            });
            _stateMachine.AddTransition(suspiciousState, searchState, () =>
            {
                /*Debug.Log($"Susp to Search {SuspiciousTimer <= 0 && detected == null}");*/
                return SuspiciousTimer <= 0 && VideoDetected == null;
            });
            _stateMachine.AddTransition(alertState, attackState, () =>
            {
                /*Debug.Log($"Alert to Attack {AlertTimer <= 0 && detected != null}");*/
                return AlertTimer <= 0 && VideoDetected != null;
            });
            _stateMachine.AddTransition(alertState, searchState, () =>
            {
                /*Debug.Log($"Alert to Search {AlertTimer <= 0 && detected == null}");*/
                return AlertTimer <= 0 && VideoDetected == null;
            });
            _stateMachine.AddTransition(attackState, searchState, () =>
            {
                /*Debug.Log($"Attack to Search {detected == null}");*/
                return VideoDetected == null;
            });
            _stateMachine.AddTransition(searchState, suspiciousState, () =>
            {
                /*Debug.Log($"Search to Idle {SearchTimer <= 0}");*/
                return VideoDetected != null || AudioDetected != null;
            });

            _stateMachine.SetInitialState(idleState);
        }

        private void OnEnable()
        {
            DetectionSystemEvents.OnAudioDetectEnter += OnAudioDetectionEnter;
            DetectionSystemEvents.OnVisionDetectEnter += OnVideoDetectionEnter;
            DetectionSystemEvents.OnAudioDetectExit += OnAudioDetectionExit;
            DetectionSystemEvents.OnVisionDetectExit += OnVideoDetectionExit;
        }

        private void OnDisable()
        {
            DetectionSystemEvents.OnAudioDetectEnter -= OnAudioDetectionEnter;
            DetectionSystemEvents.OnVisionDetectEnter -= OnVideoDetectionEnter;
            DetectionSystemEvents.OnAudioDetectExit -= OnAudioDetectionExit;
            DetectionSystemEvents.OnVisionDetectExit -= OnVideoDetectionExit;
        }

        private void OnAnimatorMove()
        {
            Vector3 position = AnimatorComponent.rootPosition;
            position.y = NavMeshAgent.nextPosition.y;
            transform.position = position;

            if (Vector3.Distance(transform.position, NavMeshAgent.nextPosition) > NavMeshAgent.radius)
            {
                //if (PullAvatarTowardsAgent) {
                //    transform.position += (NavMeshAgent.nextPosition - transform.position) * 0.1f;
                //} else if (PullAgentTowardsAvatar) {
                NavMeshAgent.nextPosition += (transform.position - NavMeshAgent.nextPosition) * 0.1f;
                //}
            }
        }

        private void OnVideoDetectionEnter(GameObject detecter, GameObject detected)
        {
            //Ho rilevato io
            if (detecter == gameObject)
            {
                //Non ho un target
                //OPPURE
                //Lo considero solo se sono in Susp
                if (VideoDetected == null || SuspiciousTimer <= SuspiciousTime && SuspiciousTimer > 0)
                {
                    //Setto detected e target
                    VideoDetected = detected;
                    DetectedVideoGO.SetActive(true);

                    //Attivo il suspTimer
                    //SuspiciousTimer = SuspiciousTime;
                }
            }
        }

        private void OnVideoDetectionExit(GameObject detecter, GameObject detected)
        {
            //Ho rilevato io e stavo già rilevando detected
            if (detecter == gameObject && VideoDetected != null && VideoDetected == detected)
            {
                if (CurrentState == EnemyFSMState.Attack) {
                    Debug.Log("Perdo oggetto");
                }
                //Rimuovo il detected
                VideoDetected = null;
                DetectedVideoGO.SetActive(false);
            }

        }

        private void OnAudioDetectionEnter(GameObject detecter, GameObject detected)
        {
            //Ho rilevato io
            if (detecter == gameObject)
            {
                //Non ho un target
                //OPPURE
                //Lo considero solo se sono in Susp
                if (AudioDetected == null || SuspiciousTimer <= SuspiciousTime && SuspiciousTimer > 0)
                {
                    //Setto detected e target
                    AudioDetected = detected;
                    DetectedAudioGO.SetActive(true);

                    //Attivo il suspTimer
                    //SuspiciousTimer = SuspiciousTime;
                }
            }
        }

        private void OnAudioDetectionExit(GameObject detecter, GameObject detected)
        {
            //Ho rilevato io e stavo già rilevando detected
            if (detecter == gameObject && AudioDetected != null && AudioDetected == detected)
            {
                //Rimuovo il detected
                AudioDetected = null;
                DetectedAudioGO.SetActive(false);
            }

        }

        private void Update() => _stateMachine.Update();//transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, RotationSpeed * Time.deltaTime);//transform.position = Vector3.Lerp(transform.position, Target, MovementSpeed * Time.deltaTime);

        public void Damaged()
        {
            life = 0;
        }
    }
}
