using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace HNC
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputHandler _input;

        [Header("Movement")]
        [Tooltip("Move speed of the characte")]
        public float MovementSpeed = 2.0f;
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;
        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;
        [Tooltip("Threshold for Controller")]
        public float Threshold = 0.2f;
        [Tooltip("Move speed of the characte")]
        public float CrouchSpeed = 1.0f;

        [Header("Animation")]
        public string MoveParamName;
        public string SpeedParamName;
        public string CrouchParamName;
        public string DeathParamName;

        [Header("Camera")]
        [SerializeField] private Transform followTarget;
        [SerializeField] private CinemachineVirtualCamera moveCamera;
        [SerializeField] private CinemachineVirtualCamera aimCamera;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float rotationDelta = 0.1f;

        private StateMachine _stateMachine;
        private List<IState> _states;
        private Vector2 _look;
        private bool _aiming;

        //Need for State
        [HideInInspector]
        public CharacterController CharacterController { get; private set; }
        [HideInInspector]
        public bool IsMoving { get; private set; }
        [HideInInspector]
        public bool IsCrouching { get; private set; }
        [HideInInspector]
        public Vector3 Input { get; private set; }
        [HideInInspector]
        public Animator Animator { get; private set; }
        public bool HasAnimator => Animator != null;
        [HideInInspector]
        public int AnimatorMoveHash { get; private set; }
        [HideInInspector]
        public int AnimatorSpeedHash { get; private set; }
        [HideInInspector]
        public int AnimatorCrouchHash { get; private set; }
        public bool Dead { get; private set; } = false;

        #region Events
        public event UnityAction DeadEvent;
        #endregion

        private void Awake()
        {
            _stateMachine = new StateMachine();

            _states = new List<IState>();
            IState idle = new PlayerIdle(this);
            _states.Add(idle);
            IState move = new PlayerMove(this);
            _states.Add(move);
            IState death = new PlayerDeath(this);
            _states.Add(death);

            _stateMachine.AddTransition(idle, move, () => IsMoving);
            _stateMachine.AddTransition(move, idle, () => !IsMoving);

            _stateMachine.AddAnyTransition(death, () => Dead);

            _stateMachine.SetInitialState(idle);
        }

        private void OnEnable()
        {
            CharacterController = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
            AnimatorMoveHash = MoveParamName.GetHashCode();
            AnimatorSpeedHash = SpeedParamName.GetHashCode();
            AnimatorCrouchHash = CrouchParamName.GetHashCode();

            _input.EnablePlayerInput();
            _input.move += OnMove;
            _input.crouchStarted += OnCrouchStarted;
            _input.crouchCanceled += OnCrouchCanceled;
            _input.look += OnLook;
            _input.aimStarted += OnAimStarted;
            _input.aimCanceled += OnAimCanceled;

            DeadEvent += OnDead;
        }

        private void OnDisable()
        {
            _input.move -= OnMove;
            _input.crouchStarted -= OnCrouchStarted;
            _input.crouchCanceled -= OnCrouchCanceled;
            _input.look -= OnLook;
            _input.aimStarted -= OnAimStarted;
            _input.aimCanceled -= OnAimCanceled;

            DeadEvent -= OnDead;
        }
        private void OnMove(Vector2 input)
        {
            input.Normalize();
            Input = new Vector3(input.x, 0, input.y);
            IsMoving = input.sqrMagnitude != 0;
        }

        private void OnCrouchStarted() => IsCrouching = true;
        private void OnCrouchCanceled() => IsCrouching = false;
        private void OnLook(Vector2 look) => _look = look;
        private void OnAimStarted()
        {
            _aiming = true;
            CameraSwitch(_aiming);
        }
        private void OnAimCanceled()
        {
            _aiming = false;
            CameraSwitch(_aiming);
        }

        private void Update()
        {
            // Horizontal - Rotate follow target around y
            followTarget.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationSpeed, Vector3.up);

            //  Vertical - Rotate follow target around x and clamp
            followTarget.transform.rotation *= Quaternion.AngleAxis(_look.y * rotationSpeed, Vector3.right);
            var angles = followTarget.transform.localEulerAngles;
            if (angles.x > 50 && angles.x < 180)
            {
                angles.x = 50;
            }
            else if (angles.x > 180 && angles.x < 330)
            {
                angles.x = 330;
            }
            angles.z = 0;
            followTarget.transform.localEulerAngles = angles;

            // TODO: should we move when aiming?
            if (_aiming)
            {
                transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
                followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }

            // Rotate
            transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
            followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);


            if (_stateMachine != null)
            {
                _stateMachine.Update();
            }
        }

        private void OnDead() => Dead = true;

        private void CameraSwitch(bool aim)
        {
            moveCamera.Priority = aim ? -1 : 1;
            aimCamera.Priority = aim ? 1 : -1;
        }
    }
}