using System.Collections.Generic;
using UnityEngine;

namespace HNC {
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
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

        private CharacterController _characterController;
        private Animator _animator;
        private StateMachine _stateMachine;
        private List<IState> _states;

        private bool _isMoving;

        private void OnEnable() {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();

            _stateMachine = new StateMachine();

            _states = new List<IState>();
            IState idle = new PlayerIdle(_animator);
            _states.Add(idle);
            idle.OnEnable();
            IState move = new PlayerMove(_input, _animator, _characterController, MovementSpeed, RotationSmoothTime, SpeedChangeRate);
            _states.Add(move);
            move.OnEnable();


            _input.EnablePlayerInput();
            _input.move += CheckMoving;

            _stateMachine.AddTransition(idle, move, () => _isMoving);
            _stateMachine.AddTransition(move, idle, () => !_isMoving);

            _stateMachine.SetInitialState(idle);
        }

        private void CheckMoving(Vector2 input) {
            _isMoving = input.sqrMagnitude != 0;
        }

        private void Update() {
            if (_stateMachine != null) {
                _stateMachine.Update();
            }
        }

        private void OnDisable() {
            _input.move -= CheckMoving;
            foreach (IState state in _states) {
                state.OnDisable();
            }
        }
    }
}