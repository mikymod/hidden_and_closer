using UnityEngine;

namespace HNC {
[RequireComponent(typeof(SphereCollider))]
    public class Detecter : MonoBehaviour {
        [Range(0.0f, 360f)]
        public float VisionAngle = 30;
        public LayerMask VisionLayerMask;

        private float _radius;
        private SphereCollider _collider;
        private Ray _ray;
        private RaycastHit _hit;

        //REMOVE
        private AudioSource _audioSource;

        private void Awake() {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            _radius = _collider.radius;
        }

        private void OnTriggerStay(Collider other) {
            //TODO CHECK IF OTHER IS PLAYER
            Vector3 distance = other.transform.position - transform.position;
            if (Mathf.Acos(Vector3.Dot(distance.normalized, transform.forward)) * Mathf.Rad2Deg <= VisionAngle * 0.5f) {
                Debug.Log($"Angle is {Mathf.Acos(Vector3.Dot(distance.normalized, transform.forward))}");
                _ray = new Ray(transform.position, distance);
                Debug.DrawLine(_ray.origin, other.transform.position, Color.green);
                if (Physics.Raycast(_ray, out _hit, _radius, VisionLayerMask)) {
                    if (_hit.collider == other) {
                        DetectionSystemEvents.OnVisionDetect.Invoke(gameObject, other.gameObject);
                    }
                }
            }
            if (other.TryGetComponent(out _audioSource) && _audioSource.isPlaying) {
                DetectionSystemEvents.OnAudioDetect.Invoke(gameObject, other.gameObject);
            }
        }
    }
}