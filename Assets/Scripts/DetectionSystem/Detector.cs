using System.Collections.Generic;
using UnityEngine;

internal enum DetectedState {
    None,
    Releved,
    FirstNotified,
    LastNotified,
}

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
        private DetectedState _playerState = DetectedState.None;

        private List<Transform> _soundEmittersTransform;
        private Dictionary<Transform, DetectedState> _soundEmittersState;

        private Ray _ray;
        private RaycastHit _hit;
        private Vector3 _distance;

        private void Awake() {
            _soundEmittersTransform = new List<Transform>();
            _soundEmittersState = new Dictionary<Transform, DetectedState>();

            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            _radius = _collider.radius;
        }

        private void OnEnable() {
            AudioManager.OnSoundPlay += AddSoundEmitter;
            AudioManager.OnSoundStop += RemoveSoundEmitter;

        }


        private void OnDisable() {
            AudioManager.OnSoundPlay -= AddSoundEmitter;
            AudioManager.OnSoundStop -= RemoveSoundEmitter;
        }

        private void OnTriggerEnter(Collider other) {
            //Save reference to Player Transform
            if (other.gameObject.layer == LayerMask.NameToLayer(PlayerLayerName) && _playerTransform == null) {
                _playerTransform = other.transform;
                _playerState = DetectedState.Releved;
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
                            switch (_playerState) {
                                case DetectedState.None:
                                    Debug.LogWarning("Error in detection System, not releved player was shoot by raycast", gameObject);
                                    break;
                                case DetectedState.Releved:
                                    DetectionSystemEvents.OnVisionDetectEnter?.Invoke(gameObject, _playerTransform.gameObject);
                                    _playerState = DetectedState.FirstNotified;
                                    break;
                                case DetectedState.FirstNotified:
                                    DetectionSystemEvents.OnVisionDetectStay?.Invoke(gameObject, _playerTransform.gameObject);
                                    break;
                                case DetectedState.LastNotified:
                                    DetectionSystemEvents.OnVisionDetectEnter?.Invoke(gameObject, _playerTransform.gameObject);
                                    _playerState = DetectedState.FirstNotified;
                                    break;
                                default:
                                    break;
                            }
                        } else {
                            if (_playerState == DetectedState.FirstNotified) {
                                DetectionSystemEvents.OnVisionDetectExit?.Invoke(gameObject, _playerTransform.gameObject);
                                _playerState = DetectedState.LastNotified;
                            }
                        }
                    } else {
                        if (_playerState == DetectedState.FirstNotified) {
                            DetectionSystemEvents.OnVisionDetectExit?.Invoke(gameObject, _playerTransform.gameObject);
                            _playerState = DetectedState.LastNotified;
                        }
                    }
                } else {
                    if (_playerState == DetectedState.FirstNotified) {
                        DetectionSystemEvents.OnVisionDetectExit?.Invoke(gameObject, _playerTransform.gameObject);
                        _playerState = DetectedState.LastNotified;
                    }
                }
            }
            //Audio detected
            for (int i = 0; i < _soundEmittersTransform.Count; i++) {
                //Check distance
                _distance = _soundEmittersTransform[i].position - transform.position;
                if (_distance.sqrMagnitude < _radius * _radius) {
                    switch (_soundEmittersState[_soundEmittersTransform[i]]) {
                        case DetectedState.None:
                            Debug.LogWarning("Error in detection System, sound NONE was releved", gameObject);
                            break;
                        case DetectedState.Releved:
                            DetectionSystemEvents.OnAudioDetectEnter?.Invoke(gameObject, _soundEmittersTransform[i].gameObject);
                            _soundEmittersState[_soundEmittersTransform[i]] = DetectedState.FirstNotified;
                            break;
                        case DetectedState.FirstNotified:
                            DetectionSystemEvents.OnAudioDetectStay?.Invoke(gameObject, _soundEmittersTransform[i].gameObject);
                            break;
                        case DetectedState.LastNotified:
                            DetectionSystemEvents.OnAudioDetectEnter?.Invoke(gameObject, _soundEmittersTransform[i].gameObject);
                            _soundEmittersState[_soundEmittersTransform[i]] = DetectedState.FirstNotified;
                            break;
                        default:
                            break;
                    }
                } else {
                    if (_soundEmittersState[_soundEmittersTransform[i]] == DetectedState.FirstNotified) {
                        DetectionSystemEvents.OnAudioDetectExit?.Invoke(gameObject, _soundEmittersTransform[i].gameObject);
                        _soundEmittersState[_soundEmittersTransform[i]] = DetectedState.LastNotified;
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (_playerTransform != null && other.transform == _playerTransform) {
                if (_playerState == DetectedState.FirstNotified) {
                    DetectionSystemEvents.OnVisionDetectExit?.Invoke(gameObject, _playerTransform.gameObject);
                }
                _playerTransform = null;
                _playerState = DetectedState.None;
            }
        }

        private void AddSoundEmitter(AudioClipsBankSO uslessACB, AudioConfigurationSO uslessAC, Transform emitter, float uslessF) {
            if (_soundEmittersTransform.Contains(emitter)) {
                return;
            }
            _soundEmittersTransform.Add(emitter);
            _soundEmittersState.Add(emitter, DetectedState.Releved);
        }

        private void RemoveSoundEmitter(Transform emitter, float usless) {
            if (!_soundEmittersTransform.Contains(emitter)) {
                return;
            }
            if (_soundEmittersState[emitter] == DetectedState.FirstNotified) {
                DetectionSystemEvents.OnAudioDetectExit?.Invoke(gameObject, emitter.gameObject);
                _soundEmittersState[emitter] = DetectedState.LastNotified;
            }
            _soundEmittersTransform.Remove(emitter);
            _soundEmittersState.Remove(emitter);
        }
    }
}