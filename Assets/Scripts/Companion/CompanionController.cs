using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class CompanionController : MonoBehaviour
    {
        [SerializeField] private InputHandler input;
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private float rotationSpeed = 5;
        [SerializeField] private CinemachineVirtualCamera moveCamera;
        [SerializeField] private CinemachineVirtualCamera companionCamera;
        [SerializeField] private Transform followTarget;
        private bool isControllingCompanion;

        private Rigidbody _rb;
        private Vector2 _move;
        private Vector2 _look;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            input.DisableAllInput();
            input.EnableCompanionInput();
            isControllingCompanion = true;
            CameraSwitch(isControllingCompanion);
            input.companionMove += OnMove;
            input.companionLook += OnLook;
            input.playerSwitch += OnCompanionControllingFinished;
        }

        private void OnDisable()
        {
            input.companionMove -= OnMove;
            input.companionLook -= OnLook;
            input.playerSwitch -= OnCompanionControllingFinished;
        }

        private void OnMove(Vector2 move) => _move = move;
        private void OnLook(Vector2 look) => _look = look;

        private void OnCompanionControllingFinished()
        {
            input.DisableAllInput();
            input.EnablePlayerInput();
            isControllingCompanion = false;
            CameraSwitch(isControllingCompanion);
            gameObject.SetActive(false);
        }

        private void CameraSwitch(bool companion)
        {
            moveCamera.Priority = isControllingCompanion ? -1 : 1;
            companionCamera.Priority = isControllingCompanion ? 1 : -1;
        }

        private void Update()
        {
            followTarget.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationSpeed, Vector3.up);
            followTarget.transform.rotation *= Quaternion.AngleAxis(_look.y * rotationSpeed, Vector3.left);
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

            if (_move != Vector2.zero)
            {
                transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
                followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }
            else
            {
                followTarget.transform.localEulerAngles = new Vector3(0, angles.y, 0);
            }

            _rb.AddForce(transform.forward * _move.y * moveSpeed);
        }
    }
}
