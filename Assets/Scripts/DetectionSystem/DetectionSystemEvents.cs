using UnityEngine;
using UnityEngine.Events;

namespace HNC {
    public class DetectionSystemEvents : MonoBehaviour {
        public static UnityAction<GameObject, GameObject> OnVisionDetect;
        public static UnityAction<GameObject, GameObject> OnAudioDetect;
    }
}
