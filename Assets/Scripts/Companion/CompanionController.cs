using System.Collections;
using Cinemachine;
using HNC.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace HNC {
    public class CompanionController : MonoBehaviour {
        [SerializeField] private InputHandler input;
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private float rotationSpeed = 5;
        [SerializeField] private CinemachineVirtualCamera companionCamera;
        [SerializeField] private Transform followTarget;
        [SerializeField] private float destroyAfter;
        private bool isControllingCompanion;

        private Rigidbody _rb;
        private Collider _collider;
        private Renderer[] _renderers;
        private Vector2 _move;
        private Vector2 _look;

        public static UnityAction OnCompanionControlStarted;
        public static UnityAction OnCompanionControlFinish;
        public static UnityAction OnCompanionDestroy;

        private AudioCarController audioController;

        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponentInChildren<Collider>();
            _renderers = GetComponentsInChildren<Renderer>();
            audioController = GetComponent<AudioCarController>();
        }

        private void OnEnable() {
            input.companionMove += OnMove;
            input.companionLook += OnLook;
            input.playerSwitch += OnCompanionControllingFinished;

            PlayerController.CompanionControl += OnCompanionControllingStarted;

            OnCompanionDestroy += DisableCar;
        }

        private void OnDisable() {
            input.companionMove -= OnMove;
            input.companionLook -= OnLook;
            input.playerSwitch -= OnCompanionControllingFinished;

            PlayerController.CompanionControl -= OnCompanionControllingStarted;

            OnCompanionDestroy -= DisableCar;
        }

        private void OnMove(Vector2 move) => _move = move;
        private void OnLook(Vector2 look) => _look = look;

        private void OnCompanionControllingStarted(Transform spawn) {
            isControllingCompanion = true;

            input.EnableCompanionInput();

            transform.SetPositionAndRotation(spawn.position, spawn.rotation);

            foreach (Renderer rend in _renderers) {
                rend.enabled = true;
            }

            _collider.enabled = true;

            CameraSwitch();
            OnCompanionControlStarted?.Invoke();
        }

        private void OnCompanionControllingFinished() {
            isControllingCompanion = false;

            input.EnablePlayerInput();

            foreach (Renderer rend in _renderers) {
                rend.enabled = false;
            }
            _collider.enabled = false;

            CameraSwitch();
            OnCompanionControlFinish?.Invoke();
        }

        private void CameraSwitch() => companionCamera.Priority = isControllingCompanion ? 20 : -20;

        private void Update() {
            followTarget.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationSpeed, Vector3.up);
            Vector3 angles = followTarget.transform.localEulerAngles;
            angles.z = 0;
            followTarget.transform.localEulerAngles = angles;

            if (_move != Vector2.zero) {
                transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
                followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
                audioController.PlayMoveSound();
            } else if (_move == Vector2.zero) {
                followTarget.transform.localEulerAngles = new Vector3(0, angles.y, 0);
                audioController.StopMoveSound();
            }

            transform.position += transform.forward * _move.y * moveSpeed * Time.deltaTime;
        }

        private void DisableCar() {
            SaveSystem.CompanionSave?.Invoke(false);
            enabled = false;
            audioController.enabled = false;
            StartCoroutine(DestroyCar());
        }

        private IEnumerator DestroyCar() {
            yield return new WaitForSeconds(destroyAfter);
            gameObject.SetActive(false);

            isControllingCompanion = false;

            input.EnablePlayerInput();

            foreach (Renderer rend in _renderers) {
                rend.enabled = false;
            }

            OnCompanionControlFinish?.Invoke();
        }

    }
}
