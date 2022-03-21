using UnityEngine;

namespace HNC {

    public class PlayerMove : IState {
        private readonly PlayerController _player;
        private float _speed;
        private float _targetRotation;
        private float _rotationVelocity = 0;
        private float _animationBlend;

        public PlayerMove(PlayerController player) => _player = player;

        public void Enter() { }

        void IState.Update() {
            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_player.CharacterController.velocity.x, 0.0f, _player.CharacterController.velocity.z).magnitude;
            float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < _player.MovementSpeed - speedOffset || currentHorizontalSpeed > _player.MovementSpeed + speedOffset) {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, _player.MovementSpeed, Time.deltaTime * _player.SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            } else {
                _speed = _player.MovementSpeed;
            }
            _animationBlend = Mathf.Lerp(_animationBlend, _player.MovementSpeed, Time.deltaTime * _player.SpeedChangeRate);

            // normalise input direction
            Vector3 inputDirection = new Vector3(_player.Input.x, 0.0f, _player.Input.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving

            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(_player.CharacterController.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _player.RotationSmoothTime);

            // rotate to face input direction relative to camera position
            _player.CharacterController.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _player.CharacterController.Move(targetDirection.normalized * (_speed * Time.deltaTime));

            //Set anim params
            if (_player.HasAnimator) {
                _player.Animator.SetBool(_player.AnimatorMoveHash, _player.Input.sqrMagnitude > _player.Threshold);
                _player.Animator.SetFloat(_player.AnimatorSpeedHash, _animationBlend);
            }
        }

        public void Exit() { }

    }
}
