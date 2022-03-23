using System.Collections.Generic;
using UnityEngine;

namespace HNC {
    [RequireComponent(typeof(SphereCollider))]
    public class Detector : MonoBehaviour {
        [Range(0.0f, 360f)]
        public float VisionAngle = 30;
        public LayerMask VisionLayerMask;
        public string PlayerLayerName;

        private float _radius;
        private SphereCollider _collider;
        private Transform _playerTransform;
        private List<Transform> _audioEmitter;
        private List<Transform> _senderAudioEmitter;
        private Ray _ray;
        private RaycastHit _hit;
        private Vector3 _distance;

        private void Awake() {
            _audioEmitter = new List<Transform>();
            _senderAudioEmitter = new List<Transform>();
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            _radius = _collider.radius;
        }

        private void OnEnable() => AudioManager.OnDoingNoise += CheckAudio;

        private void OnDisable() => AudioManager.OnDoingNoise -= CheckAudio;

        private void OnTriggerEnter(Collider other) {
            //Save reference to Player Transform
            if (other.gameObject.layer == LayerMask.NameToLayer(PlayerLayerName)) {
                _playerTransform = other.transform;
            }
        }

        private void Update() {
            //Vision detected
            if (_playerTransform != null) {
                Vector3 distance = _playerTransform.position - transform.position;
                if (Mathf.Acos(Vector3.Dot(distance.normalized, transform.forward)) * Mathf.Rad2Deg <= VisionAngle * 0.5f) {
                    _ray = new Ray(transform.position, distance);
                    Debug.DrawLine(_ray.origin, _playerTransform.position, Color.green);
                    if (Physics.Raycast(_ray, out _hit, _radius, VisionLayerMask)) {
                        if (_hit.collider.transform == _playerTransform) {
                            DetectionSystemEvents.OnVisionDetect.Invoke(gameObject, _playerTransform.gameObject);
                        }
                    }
                }
            }
            //Audio detected
            for (int i = _audioEmitter.Count -1 ; i >= 0; i--) {
                //Check distance
                _distance = _audioEmitter[i].position - transform.position;
                if (_distance.sqrMagnitude < _radius * _radius) {
                    Debug.Log("Send Audio even", _audioEmitter[i].gameObject);
                    DetectionSystemEvents.OnAudioDetect.Invoke(gameObject, _audioEmitter[i].gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other) => _playerTransform = null;

        private void CheckAudio(Transform emitter) {
            Debug.Log("Recive audio event");
            if (_audioEmitter.Contains(emitter)) {
                return;
            }
            _audioEmitter.Add(emitter);
        }
    }
}