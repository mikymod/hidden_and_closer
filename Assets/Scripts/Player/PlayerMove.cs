using UnityEngine;

namespace HNC {

    public class PlayerMove : IState {
        private readonly InputHandler _inputHandler;
        private readonly Animator _animator;
        private readonly CharacterController _characterController;
        private readonly float _movementSpeed;
        private readonly float _rotationSmoothTime;
        private readonly float _speedChangeRate;

        private Vector2 _input;
        private float _speed;
        private float _targetRotation;
        private float _rotationVelocity = 0;
        private float _animationBlend;

        public PlayerMove(InputHandler input, Animator animator, CharacterController characterController, float movementSpeed, float rotationSmoothTime, float speedChangeRate) {
            _inputHandler = input;
            _animator = animator;
            _characterController = characterController;
            _movementSpeed = movementSpeed;
            _rotationSmoothTime = rotationSmoothTime;
            _speedChangeRate = speedChangeRate;
        }

        public void OnEnable() => _inputHandler.move += OnMove;

        public void Enter() {
            if (_animator != null) {
                _animator.SetBool("Move", true);
            }
        }

        private void OnMove(Vector2 input) => _input = input;

        void IState.Update() {
            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;
            float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < _movementSpeed - speedOffset || currentHorizontalSpeed > _movementSpeed + speedOffset) {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, _movementSpeed, Time.deltaTime * _speedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            } else {
                _speed = _movementSpeed;
            }
            _animationBlend = Mathf.Lerp(_animationBlend, _movementSpeed, Time.deltaTime * _speedChangeRate);

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.x, 0.0f, _input.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving

            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(_characterController.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotationSmoothTime);

            // rotate to face input direction relative to camera position
            _characterController.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime));

            if (_animator != null) {
                _animator.SetFloat("Speed", _animationBlend);
            }
        }
        public void Exit() { }

        public void OnDisable() => _inputHandler.move -= OnMove;

    }
}
