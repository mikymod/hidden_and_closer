using UnityEngine;
using UnityEngine.Events;

namespace HNC {
    public class DetectionSystemEvents : MonoBehaviour {
        public static UnityAction<GameObject, GameObject> OnVisionDetectEnter;
        public static UnityAction<GameObject, GameObject> OnVisionDetectStay;
        public static UnityAction<GameObject, GameObject> OnVisionDetectExit;
        public static UnityAction<GameObject, GameObject> OnAudioDetectEnter;
        public static UnityAction<GameObject, GameObject> OnAudioDetectStay;
        public static UnityAction<GameObject, GameObject> OnAudioDetectExit;
    }
}
