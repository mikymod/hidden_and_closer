using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HNC
{
    [RequireComponent(typeof(Pooler))]
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
        [SerializeField] private int simSteps = 120;

        private Animator _animator;
        private Pooler _pooler;
        private GameObject _bullet;
        private Rigidbody _bulletRB;
        private LineRenderer _bulletLineRenderer;
        private bool _aiming;
        private bool _fire;

        private Scene _physicsScene;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _pooler = GetComponent<Pooler>();
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

        private void Start()
        {
            CreatePhysicsScene();
        }

        private void Update()
        {
            SimulatePath();
        }

        private void OnAimStarted()
        {
            _aiming = true;

            CameraSwitch(_aiming);

            // Active bullet and bind to player's right hand
            _bullet = _pooler.Get();
            _bulletRB = _bullet.GetComponent<Rigidbody>();
            _bulletLineRenderer = _bullet.GetComponent<LineRenderer>();
            _bullet.transform.parent = handTransform;
            _bullet.transform.position = bulletTransform.position;
            _bulletRB.isKinematic = true;
            _bulletRB.useGravity = false;

            EnableAimAnimationLayer();
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
            _bulletLineRenderer.enabled = false;
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

            // Shoot the bullet
            _bullet.transform.parent = null;
            _bulletRB.isKinematic = false;
            _bulletRB.useGravity = true;
            _bulletRB.AddForce(followTarget.forward * fireForce, ForceMode.Impulse);

            // Wait till animation end
            var state = _animator.GetCurrentAnimatorStateInfo(1);
            // Retrieve only the decimal part. See this: https://docs.unity3d.com/ScriptReference/AnimatorStateInfo-normalizedTime.html
            var time = state.normalizedTime - Math.Truncate(state.normalizedTime);

            _aiming = false;

            yield return new WaitForSeconds(state.length);

            CameraSwitch(_aiming);
            DisableAimAnimationLayer();
        }

        private void EnableAimAnimationLayer() => _animator.SetLayerWeight(1, 1);
        private void DisableAimAnimationLayer() => _animator.SetLayerWeight(1, 0);

        private void CreatePhysicsScene()
        {
            _physicsScene = SceneManager.CreateScene("physics-scene", new CreateSceneParameters(LocalPhysicsMode.Physics3D));

            var root = Instantiate(SceneManager.GetActiveScene().GetRootGameObjects()[0]);
            root.GetComponentInChildren<AimController>().enabled = false;
            var renderers = root.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.enabled = false;
            }

            var newRoot = new GameObject("Physics Root");
            var physicsObjects = root.GetComponentsInChildren<Collider>();
            foreach (var obj in physicsObjects)
            {
                obj.transform.SetParent(newRoot.transform);
            }

            SceneManager.MoveGameObjectToScene(newRoot, _physicsScene);

            Destroy(root);
        }

        private void SimulatePath()
        {
            if (_bulletLineRenderer == null)
            {
                return;
            }

            if (!_aiming)
            {
                _bulletLineRenderer.positionCount = 0;
                return;
            }

            _bulletLineRenderer.positionCount = 0;

            // Create simulated bullet and add to physics scene
            var simBullet = Instantiate(bulletPrefab, _bullet.transform.position, _bullet.transform.rotation);
            SceneManager.MoveGameObjectToScene(simBullet, _physicsScene);
            simBullet.SetActive(true);

            // Simulate a specific number of frames into the future
            var simBulletRB = simBullet.GetComponent<Rigidbody>();
            simBulletRB.isKinematic = false;
            simBulletRB.useGravity = true;
            simBulletRB.AddForce(followTarget.forward * fireForce, ForceMode.Impulse);

            // Draw
            _bulletLineRenderer.enabled = true;
            _bulletLineRenderer.positionCount = simSteps;
            for (int i = 0; i < simSteps; i++)
            {
                _physicsScene.GetPhysicsScene().Simulate(Time.fixedDeltaTime);
                _bulletLineRenderer.SetPosition(i, simBullet.transform.position);
            }

            Destroy(simBullet);
        }
    }

}

