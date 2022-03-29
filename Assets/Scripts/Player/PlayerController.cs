using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace HNC
{
    // Player Controller semplificato 
    public class PlayerController : MonoBehaviour {
        [Header("Input")]
        [Header("Input SO")]
        [SerializeField] private InputHandler input;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;
        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;
        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;
        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;


        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private readonly float _terminalVelocity = 53.0f;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDAim;
        private int _animIDShoot;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private Animator _animator;
        private CharacterController _controller;
        private GameObject _mainCamera;

        private void Awake() {
            // get a reference to our main camera
            if (_mainCamera == null) {
                //_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                _mainCamera = Camera.main.gameObject;
            }
        }

        private void Start() {
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();

            AssignAnimationIDs();
        }

        private void Update() {
            _hasAnimator = TryGetComponent(out _animator);

            GroundedCheck();
            Shoot();
            Move();
        }

        private void LateUpdate() => CameraRotation();

        private void AssignAnimationIDs() {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDAim = Animator.StringToHash("Aim");
            _animIDShoot = Animator.StringToHash("Shoot");
        }

        private void CameraRotation() {
            // if there is an input and camera position is not fixed
            if (_look.sqrMagnitude >= _threshold && !LockCameraPosition) {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = 1.0f ;

                _cinemachineTargetYaw += _look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }

        private void Move() {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_move == Vector2.zero) {
                targetSpeed = 0.0f;
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset) {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            } else {
                _speed = targetSpeed;
            }
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero || _input.aim) {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _input.aim ? _mainCamera.transform.eulerAngles.y : _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator) {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax) {
            if (lfAngle < -360f) {
                lfAngle += 360f;
            }

            if (lfAngle > 360f) {
                lfAngle -= 360f;
            }

            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        //[SerializeField] private Transform followTarget;
        //[SerializeField] private float rotationSpeed = 1f;
        //[SerializeField] private float rotationDelta = 0.1f;
        //[SerializeField] private float moveSpeed = 1f;
        //[SerializeField] public float crouchSpeed = 1.0f;
        //[SerializeField] private Transform companionSpot;

        #region Events
        public event UnityAction DeadEvent;
        public static UnityAction<Transform> companionControl;
        #endregion

        //private CharacterController _character;
        //private Animator _animator;
        private Vector2 _move;
        private Vector2 _look;
        private bool _aiming;
        private bool _crouch;
        private bool _dead;
        private bool _fire;

        //private void Awake()
        //{
        //    _character = GetComponent<CharacterController>();
        //    _animator = GetComponent<Animator>();
        //}

        private void OnEnable()
        {
            input.DisableAllInput();
            input.EnablePlayerInput(); // FIXME: this should not be here

            input.move += OnMove;
            input.look += OnLook;
            input.crouchStarted += OnCrouchStarted;
            input.aimStarted += OnAimStarted;
            input.aimCanceled += OnAimCanceled;
            input.companionSwitch += OnCompanionControllingStarted;

            DeadEvent += OnDeath;
        }

        private void OnDisable()
        {
            input.move -= OnMove;
            input.look -= OnLook;
            input.crouchStarted -= OnCrouchStarted;
            input.aimStarted -= OnAimStarted;
            input.aimCanceled -= OnAimCanceled;
            input.companionSwitch -= OnCompanionControllingStarted;

            DeadEvent -= OnDeath;
        }

        private void OnMove(Vector2 move) => _move = move;
        private void OnLook(Vector2 look) => _look = look;
        private void OnAimStarted() => _aiming = true;
        private void OnAimCanceled() => _aiming = false;
        private void OnCrouchStarted() => _crouch = !_crouch;
        private void OnDeath() => _animator.SetTrigger("Death");
        private void OnCompanionControllingStarted()
        {
            input.DisableAllInput();
            input.EnableCompanionInput();

            companionControl?.Invoke(companionSpot);
        }
        //private void Update()
        //{
        //    // Horizontal - Rotate follow target around y
        //    followTarget.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationSpeed, Vector3.up);

        //    //  Vertical - Rotate follow target around x and clamp
        //    followTarget.transform.rotation *= Quaternion.AngleAxis(_look.y * rotationSpeed, Vector3.left);
        //    var angles = followTarget.transform.localEulerAngles;
        //    if (angles.x > 50 && angles.x < 180)
        //    {
        //        angles.x = 50;
        //    }
        //    else if (angles.x > 180 && angles.x < 330)
        //    {
        //        angles.x = 330;
        //    }
        //    angles.z = 0;
        //    followTarget.transform.localEulerAngles = angles;

        //    // Move
        //    float speed = (_crouch ? crouchSpeed : moveSpeed) / 50f;
        //    Vector3 motion = (transform.forward * _move.y * speed) + (transform.right * _move.x * speed);
        //    _character.Move(motion);

        //    if (_aiming || _move != Vector2.zero)
        //    {
        //        transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
        //        followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        //    }
        //    else if (_move == Vector2.zero)
        //    {
        //        followTarget.transform.localEulerAngles = new Vector3(0, angles.y, 0);
        //    }

        //    _animator.SetFloat("Speed", 1);
        //    _animator.SetBool("Move", _move.magnitude > 0.2f);
        //    _animator.SetBool("Crouch", _crouch);
        //}
    }
}
