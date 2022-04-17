using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HNC.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace HNC {
    public class DetectionSystem : MonoBehaviour {
        public UnityAction<Vector3> NoiseDetected;
        public UnityAction<Transform> VisibleDetected;
        public UnityAction ExitFromVisibleArea;

        [Header("hearing")]
        public float hearingRadius;
        public LayerMask soundMask;
        public float soundVolumeThreshold = 0.5f;

        [Header("Sight")]
        public float viewRadius;
        [Range(0, 360)] public float viewAngle;
        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public List<Transform> visibleTargets = new List<Transform>(); // Debug: used in editor

        private bool eventSend = true;

        public static UnityAction AudioNoiseDetected;
        public static UnityAction AudioVisibleDetected;
        public static UnityAction AudioExitFromVisibleArea;

        private void OnEnable() => AudioEventsManager.OnSoundPlay += OnSoundPlay;

        private void OnDisable() => AudioEventsManager.OnSoundPlay -= OnSoundPlay;

        private void OnSoundPlay(AudioClipsBankSO bank, AudioConfigurationSO configuration, Transform sound, float fade) {
            if (configuration.volume <= soundVolumeThreshold || sound == null) {
                return;
            }
            Collider collider = Physics.OverlapSphere(transform.position, hearingRadius, soundMask).FirstOrDefault(collider => collider.transform.position == sound.position);
            if (collider != null) {
                NoiseDetected?.Invoke(collider.transform.position);
                AudioNoiseDetected?.Invoke();
            }
        }

        private void Start() => StartCoroutine(FindVisibleTargetsCoroutine(0.2f));

        private IEnumerator FindVisibleTargetsCoroutine(float delay) {
            while (true) {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        private void FindVisibleTargets() {
            visibleTargets.Clear();
            Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targets.Length; i++) {
                Transform target = targets[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask)) {
                        visibleTargets.Add(target);
                        VisibleDetected?.Invoke(target);
                        AudioVisibleDetected?.Invoke();
                        eventSend = false;
                    }
                }
            }

            if (!eventSend && visibleTargets.Count <= 0) {
                ExitFromVisibleArea?.Invoke();
                AudioExitFromVisibleArea?.Invoke();
                eventSend = true;
            }
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool globalAngle) {
            angleInDegrees += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }

}
