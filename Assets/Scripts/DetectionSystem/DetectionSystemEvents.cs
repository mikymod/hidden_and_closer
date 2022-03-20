using UnityEngine;
using UnityEngine.Events;

namespace HNC {
    public class DetectionSystemEvents : MonoBehaviour {
        public static UnityEvent<GameObject, GameObject> OnVisionDetect = new UnityEvent<GameObject, GameObject>();
        public static UnityEvent<GameObject, GameObject> OnAudioDetect = new UnityEvent<GameObject, GameObject>();
    }
}
