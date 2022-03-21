using System.Collections.Generic;
using UnityEngine;

namespace HNC {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour {
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


        [Header("Animation")]
        [SerializeField] private string MoveParamName;
        [SerializeField] private string SpeedParamName;
        [SerializeField] private string CrouchParamName;

        private StateMachine _stateMachine;
        private List<IState> _states;

        //Need for State
        [HideInInspector]
        public CharacterController CharacterController { get; private set; }
        [HideInInspector]
        public bool IsMoving { get; private set; }
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

        private void Awake() {

            _stateMachine = new StateMachine();

            _states = new List<IState>();
            IState idle = new PlayerIdle(this);
            _states.Add(idle);
            IState move = new PlayerMove(this);
            _states.Add(move);

            _stateMachine.AddTransition(idle, move, () => IsMoving);
            _stateMachine.AddTransition(move, idle, () => !IsMoving);

            _stateMachine.SetInitialState(idle);
        }

        private void OnEnable() {
            CharacterController = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
            AnimatorMoveHash = MoveParamName.GetHashCode();
            AnimatorSpeedHash= SpeedParamName.GetHashCode();
            AnimatorCrouchHash = CrouchParamName.GetHashCode();

            _input.EnablePlayerInput();
            _input.move += OnMove;

        }

        private void OnMove(Vector2 input) {
            input.Normalize();
            Input = new Vector3(input.x, 0, input.y);
            IsMoving = input.sqrMagnitude != 0;
        }

        private void Update() {
            if (_stateMachine != null) {
                _stateMachine.Update();
            }
        }

        private void OnDisable() {
            _input.move -= OnMove;
        }
    }
}