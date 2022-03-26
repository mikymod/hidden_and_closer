using System.Collections;
using Cinemachine;
using UnityEngine;

namespace HNC
{
    public class AimController : MonoBehaviour
    {
        [SerializeField] private InputHandler input;
        [SerializeField] private CinemachineVirtualCamera moveCamera;
        [SerializeField] private CinemachineVirtualCamera aimCamera;
        [SerializeField] private Transform bulletTransform;
        [SerializeField] private Transform handTransform;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform followTarget;
        [SerializeField] private float fireForce = 5f;
        private Animator _animator;
        private GameObject _bullet;
        private Rigidbody _bulletRB;
        private bool _aiming;
        private bool _fire;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _bullet = Instantiate(bulletPrefab, bulletTransform.position, bulletTransform.rotation, handTransform);
            _bulletRB = _bullet.GetComponent<Rigidbody>();
            _bullet.SetActive(false);
        }

        private void OnEnable()
        {
            input.aimStarted += OnAimStarted;
            input.aimCanceled += OnAimCanceled;
            input.fireStarted += OnFireStarted;
        }

        private void OnDisable()
        {
            input.aimStarted -= OnAimStarted;
            input.aimCanceled -= OnAimCanceled;
            input.fireStarted -= OnFireStarted;
        }

        private void OnAimStarted()
        {
            _aiming = true;
            CameraSwitch(_aiming);
            EnableAimAnimationLayer();

            _bullet.SetActive(true);
            _bullet.transform.parent = handTransform;
            _bullet.transform.position = bulletTransform.position;
        }

        private void OnAimCanceled()
        {
            _aiming = false;
            CameraSwitch(_aiming);
            DisableAimAnimationLayer();
            _bullet.SetActive(false);
        }

        private void OnFireStarted()
        {
            _fire = true;
            StartCoroutine(ShootCoroutine());
        }

        private void CameraSwitch(bool aim)
        {
            moveCamera.Priority = aim ? -1 : 1;
            aimCamera.Priority = aim ? 1 : -1;
        }

        private IEnumerator ShootCoroutine()
        {
            _animator.SetTrigger("Shoot");
            _bullet.transform.parent = null;
            _bulletRB.isKinematic = false;
            _bulletRB.useGravity = true;
            _bulletRB.AddForce(followTarget.forward * fireForce, ForceMode.Impulse);
            while (_animator.GetCurrentAnimatorStateInfo(1).normalizedTime != 1f)
            {
                yield return null;
            }
            DisableAimAnimationLayer();
        }

        private void EnableAimAnimationLayer() => _animator.SetLayerWeight(1, 1);
        private void DisableAimAnimationLayer() => _animator.SetLayerWeight(1, 0);
    }

}

