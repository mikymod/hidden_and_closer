using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace HNC
{
    // Player Controller semplificato 
    public class PlayerController : MonoBehaviour
    {
        [Header("Input")]
        [Header("Input SO")]
        [SerializeField] private InputHandler input;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;
        [Tooltip("Crouch speed of the character in m/s")]
        public float CrouchSpeed = 1.0f;
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

        [Header("Animation Params")]
        [Tooltip("Aim Layer")]
        public string AnimLayerAim;
        [Tooltip("Move param")]
        public string AnimIDMove;
        [Tooltip("Speed param")]
        public string AnimIDSpeed;
        [Tooltip("Crouch param")]
        public string AnimIDCrouch;
        [Tooltip("Aim param")]
        public string AnimIDAim;
        [Tooltip("Shoot param")]
        public string AnimIDShoot;
        [Tooltip("Death param")]
        public string AnimIDDeath;


        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private readonly float _verticalVelocity;
        private readonly float _terminalVelocity = 53.0f;

        // input
        private Vector2 _move;
        private Vector2 _look;
        // private bool _aim;
        private bool _crouch;

        private bool _interact;
        private bool _canInteract;

        // animation Layer
        private int _animLayerAim;

        // animation IDs
        private int _animIDMove;
        private int _animIDSpeed;
        private int _animIDCrouch;
        private int _animIDAim;
        private int _animIDShoot;
        private int _animIDDeath;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private Animator _animator;
        private CharacterController _controller;
        private GameObject _mainCamera;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                //_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                _mainCamera = Camera.main.gameObject;
            }

            input.move += OnMove;
            input.look += OnLook;
            input.crouchStarted += OnCrouchStarted;
            input.aimStarted += OnAimStarted;
            input.aimCanceled += OnAimCanceled;
            input.interactStarted += OnInteractStarted;
            input.interactCanceled += OnInteractCanceled;
            input.companionSwitch += OnCompanionControllingStarted;

            DeadEvent += OnDeath;
        }

        private void OnDestroy()
        {
            input.move -= OnMove;
            input.look -= OnLook;
            input.crouchStarted -= OnCrouchStarted;
            input.aimStarted -= OnAimStarted;
            input.aimCanceled -= OnAimCanceled;
            input.interactStarted -= OnInteractStarted;
            input.interactCanceled -= OnInteractCanceled;
            input.companionSwitch -= OnCompanionControllingStarted;

            DeadEvent -= OnDeath;
        }

        private void OnEnable()
        {
            input.DisableAllInput();
            input.EnablePlayerInput();
        }

        private void OnDisable()
        {
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();

            AssignAnimationIDs();
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            //Shoot();
            Move();
        }

        private void LateUpdate() => CameraRotation();

        private void AssignAnimationIDs()
        {
            _animIDMove = Animator.StringToHash(AnimIDMove);
            _animIDSpeed = Animator.StringToHash(AnimIDSpeed);
            _animIDCrouch = Animator.StringToHash(AnimIDCrouch);
            _animIDAim = Animator.StringToHash(AnimIDAim);
            _animIDShoot = Animator.StringToHash(AnimIDShoot);
            _animIDDeath = Animator.StringToHash(AnimIDDeath);
            if (_hasAnimator)
            {
                _animLayerAim = _animator.GetLayerIndex(AnimLayerAim);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = 1.0f;

                _cinemachineTargetYaw += _look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _crouch ? CrouchSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _move.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            // normalise input direction
            Vector3 inputDirection = new Vector3(_move.x, 0.0f, _move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                // float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _aim ? _mainCamera.transform.eulerAngles.y : _targetRotation, ref _rotationVelocity, RotationSmoothTime);
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDMove, inputMagnitude != 0);
                _animator.SetFloat(_animIDSpeed, inputMagnitude);
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f)
            {
                lfAngle += 360f;
            }

            if (lfAngle > 360f)
            {
                lfAngle -= 360f;
            }

            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        #region Events

        [SerializeField] private Transform companionSpot;

        #region Public Action
        public static UnityAction DeadEvent;
        public static UnityAction<Transform> CompanionControl;
        private bool isDeath = false;
        #endregion

        private void OnMove(Vector2 move) => _move = move;

        private void OnLook(Vector2 look) => _look = new Vector2(look.x, -look.y);

        private void OnAimStarted() => enabled = false;

        private void OnAimCanceled() => enabled = true;

        private void OnInteractStarted() => _interact = true;

        private void OnInteractCanceled() => _interact = false;

        private void OnCrouchStarted()
        {
            _crouch = !_crouch;
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDCrouch, _crouch);
            }
        }

        private void OnDeath()
        {
            if (_hasAnimator && !isDeath)
            {
                _animator.SetTrigger(_animIDDeath);
                input.DisableAllInput();
                isDeath = true;
                //TODO abilitare il menu di game over
                //TODO evento che stoppa il searching degli zombies
            }
        }

        private void OnCompanionControllingStarted()
        {
            CompanionControl?.Invoke(companionSpot);
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                if (_interact)
                {
                    Debug.Log("INTERACT ENTER");
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                if (_interact)
                {
                    var interactable = other.gameObject.GetComponentInParent<Interactable>();
                    interactable.Interact();
                }
            }
        }

    }
}
