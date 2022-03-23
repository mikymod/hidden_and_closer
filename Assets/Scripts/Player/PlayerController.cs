using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace HNC
{
    // Player Controller semplificato 
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputHandler input;
        [SerializeField] private Transform followTarget;
        [SerializeField] private CinemachineVirtualCamera moveCamera;
        [SerializeField] private CinemachineVirtualCamera aimCamera;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float rotationDelta = 0.1f;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] public float crouchSpeed = 1.0f;

        #region Events
        public event UnityAction DeadEvent;
        #endregion

        private CharacterController _character;
        private Animator _animator;
        private Vector2 _move;
        private Vector2 _look;
        private bool _aiming;
        private bool _crouch;
        private bool _dead;

        private void Awake()
        {
            _character = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            input.EnablePlayerInput(); // FIXME: this should not be here

            input.move += OnMove;
            input.look += OnLook;
            input.aimStarted += OnAimStarted;
            input.aimCanceled += OnAimCanceled;
            input.crouchStarted += OnCrouchStarted;

            DeadEvent += OnDeath;
        }

        private void OnDisable()
        {
            input.move -= OnMove;
            input.look -= OnLook;
            input.aimStarted -= OnAimStarted;
            input.aimCanceled -= OnAimCanceled;
            input.crouchStarted -= OnCrouchStarted;

            DeadEvent -= OnDeath;
        }

        private void OnMove(Vector2 move) => _move = move;
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
        private void OnCrouchStarted() => _crouch = !_crouch;

        private void OnDeath() => _animator.SetTrigger("Death");

        private void Update()
        {
            // Horizontal - Rotate follow target around y
            followTarget.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationSpeed, Vector3.up);

            //  Vertical - Rotate follow target around x and clamp
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

            //
            if (_aiming)
            {
                transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
                followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }

            // Move
            float speed = (_crouch ? crouchSpeed : moveSpeed) / 50f;
            Vector3 motion = (transform.forward * _move.y * speed) + (transform.right * _move.x * speed);
            _character.Move(motion);

            // Rotate around player if player isn't moving 
            if (_move != Vector2.zero)
            {
                transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
                followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }

            _animator.SetFloat("Speed", 1);
            _animator.SetBool("Move", _move.magnitude > 0.2f);
            _animator.SetBool("Crouch", _crouch);
        }

        private void CameraSwitch(bool aim)
        {
            moveCamera.Priority = aim ? -1 : 1;
            aimCamera.Priority = aim ? 1 : -1;
        }
    }
}
