using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace HNC
{
    [RequireComponent(typeof(Pooler))]
    [RequireComponent(typeof(LineRenderer))]
    public class AimController : MonoBehaviour
    {
        [SerializeField] private InputHandler input;
        [SerializeField] private CinemachineVirtualCamera moveCamera;
        [SerializeField] private CinemachineVirtualCamera aimCamera;
        [SerializeField] private Transform bulletTransform;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject trajectoryCollisionPoint;
        [SerializeField] private int trajectoryLinePointsCount = 50;
        [Tooltip("Crouch speed of the character in m/s")]
        [SerializeField] float CrouchSpeed = 2.0f;
        [SerializeField] LayerMask AimLayer;

        private Animator _animator;
        private Pooler _pooler;
        private LineRenderer _lineRenderer;
        private CharacterController _controller;
        private Rigidbody _bulletRB;
        private bool _aiming;
        private bool _canFire = true;
        private Vector2 _look;
        private Vector2 _move;
        private Vector3 _throwForce;
        private List<Vector3> _trajectorylinePoints = new List<Vector3>();
        private float range = 350;
        private RaycastHit _lastTrajectoryHit;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _pooler = GetComponent<Pooler>();
            _lineRenderer = GetComponent<LineRenderer>();
            _controller = GetComponent<CharacterController>();

            input.aimStarted += OnAimStarted;
            input.aimCanceled += OnAimCanceled;
            input.fireStarted += OnFireStarted;
            input.look += OnLook;
            input.move += OnMove;
        }

        private void OnDestroy()
        {
            input.aimStarted -= OnAimStarted;
            input.aimCanceled -= OnAimCanceled;
            input.fireStarted -= OnFireStarted;
            input.look -= OnLook;
            input.move -= OnMove;
        }

        private void Update()
        {
            if (_aiming)
            {
                transform.rotation *= Quaternion.Euler(0, _look.x, 0);

               
                var horizontalMove = transform.forward * _move.y + transform.right * _move.x;
                if (horizontalMove != Vector3.zero)
                {
                    _controller.Move(horizontalMove * CrouchSpeed * Time.deltaTime);
                }

                _animator.SetBool("Move", horizontalMove.magnitude != 0);
                _animator.SetFloat("Speed", horizontalMove.magnitude);

                
                range += _look.y;
                range = Mathf.Clamp(range, 200, 500);

                _throwForce = (transform.up + transform.forward) * range;
                ShowTrajectory(_throwForce, bulletTransform.position);
            }
            else
            {
                range = 350;
                HideTrajectory();
            }
        }

        private void OnAimStarted()
        {
            enabled = true;

            _aiming = true;

            CameraSwitch(_aiming);

            EnableAimAnimationLayer();

            trajectoryCollisionPoint.SetActive(true);

            _lineRenderer.enabled = true;

            Cursor.visible = false;
        }

        private void OnAimCanceled()
        {
            _aiming = false;

            CameraSwitch(_aiming);

            DisableAimAnimationLayer();

            trajectoryCollisionPoint.SetActive(false);

            _lineRenderer.positionCount = 0;

            enabled = false;
        }

        private void OnLook(Vector2 look) => _look = look;
        private void OnMove(Vector2 move) => _move = move;

        private void OnFireStarted()
        {
            if (!_aiming || !_canFire)
            {
                return;
            }

            StartCoroutine(ShootCoroutine());
        }

        private void CameraSwitch(bool aim)
        {
            moveCamera.Priority = aim ? -1 : 1;
            aimCamera.Priority = aim ? 1 : -1;
        }

        private IEnumerator ShootCoroutine()
        {
            _canFire = false;

            _animator.SetTrigger("Shoot");

            
            var bullet = _pooler.Get();
            bullet.transform.parent = null;
            bullet.transform.position = bulletTransform.position;

            
            var bulletRB = bullet.GetComponent<Rigidbody>();
            bulletRB.isKinematic = false;
            bulletRB.useGravity = true;
            bulletRB.AddForce(_throwForce);

            
            var state = _animator.GetCurrentAnimatorStateInfo(1);
            
            var time = state.normalizedTime - Math.Truncate(state.normalizedTime);
            yield return new WaitForSeconds(state.length + 1);

            _lineRenderer.positionCount = 0;
            _canFire = true;
        }

        private void EnableAimAnimationLayer()
        {
            _animator.SetBool("Aim", true);
            _animator.SetLayerWeight(1, 1);
        }
        private void DisableAimAnimationLayer()
        {
            _animator.SetBool("Aim", false);
            _animator.SetLayerWeight(1, 0);
        }

        private void ShowTrajectory(Vector3 force, Vector3 start)
        {
            var mass = 1.0f;
            Vector3 velocity = (force / mass) * Time.fixedDeltaTime;
            float flightDuration = (2 * velocity.y) / Physics.gravity.y;
            float step = flightDuration / trajectoryLinePointsCount;

            _trajectorylinePoints.Clear();

            _trajectorylinePoints.Add(start);

            for (int i = 1; i < trajectoryLinePointsCount; i++)
            {
                float timePassed = step * i;

                Vector3 move = new Vector3(
                    velocity.x * timePassed,
                    velocity.y * timePassed - 0.5f * Physics.gravity.y * (timePassed * timePassed),
                    velocity.z * timePassed
                );

                var previousPoint = _trajectorylinePoints[i - 1];
                var newPoint = start - move;
                var distance = (newPoint - previousPoint).magnitude;
                if (Physics.Raycast(previousPoint, newPoint - previousPoint, out _lastTrajectoryHit, distance, AimLayer))
                {
                    _trajectorylinePoints.Add(_lastTrajectoryHit.point);
                    _lineRenderer.positionCount = _trajectorylinePoints.Count;
                    _lineRenderer.SetPositions(_trajectorylinePoints.ToArray());
                    trajectoryCollisionPoint.transform.position = _lastTrajectoryHit.point + Vector3.up * 0.01f;
                    trajectoryCollisionPoint.transform.rotation = Quaternion.LookRotation(-_lastTrajectoryHit.normal, Vector3.up);
                    return;
                }

                _trajectorylinePoints.Add(newPoint);
            }

            var lastDirection = (_trajectorylinePoints[_trajectorylinePoints.Count - 1] - _trajectorylinePoints[_trajectorylinePoints.Count - 2]).normalized;
            var lastPoint = _trajectorylinePoints[_trajectorylinePoints.Count - 1];
            if (Physics.Raycast(lastPoint, lastDirection, out _lastTrajectoryHit, float.PositiveInfinity, AimLayer))
            {
                trajectoryCollisionPoint.transform.position = _lastTrajectoryHit.point + Vector3.up * 0.01f;
                trajectoryCollisionPoint.transform.rotation = Quaternion.LookRotation(-_lastTrajectoryHit.normal, Vector3.up);
                _trajectorylinePoints.Add(_lastTrajectoryHit.point);

            }

            _lineRenderer.positionCount = _trajectorylinePoints.Count;
            _lineRenderer.SetPositions(_trajectorylinePoints.ToArray());
        }

        private void HideTrajectory() => _lineRenderer.positionCount = 0;
    }
}

