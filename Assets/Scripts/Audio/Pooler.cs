using System.Collections.Generic;
using UnityEngine;

namespace HNC {
    public class Pooler : MonoBehaviour {
        [SerializeField] private GameObject soundEmitter;
        [SerializeField] private int soundEmittersCount;
        private List<GameObject> soundEmitters;
        private readonly List<int> keys = new List<int>();

        private void Awake() {
            soundEmitters = new List<GameObject>();
            for (int i = 0; i < soundEmittersCount; i++) {
                GameObject go = Instantiate(soundEmitter, transform);
                go.SetActive(false);
                soundEmitters.Add(go);
            }
        }

        public GameObject GetSoundEmitter() {
            for (int i = 0; i < soundEmitters.Count; i++) {
                if (!soundEmitters[i].activeInHierarchy) {
                    soundEmitters[i].SetActive(true);
                    soundEmitters[i].GetComponent<SoundEmitter>().SetUniqueID();
                    keys.Add(soundEmitters[i].GetComponent<SoundEmitter>().GetUniqueID());
                    return soundEmitters[i];
                }
            }

            GameObject go = Instantiate(soundEmitter, transform);
            go.SetActive(true);
            soundEmitters.Add(go);


            return go;
        }

        public GameObject DisposeSoundEmitter() {

            for (int i = 0; i < soundEmitters.Count; i++) {
                if (soundEmitters[i].activeInHierarchy && soundEmitters[i].GetComponent<SoundEmitter>().CheckForUniqueID(keys[i])) {
                    keys.RemoveAt(i);
                    GameObject go = soundEmitters[i];
                    go.SetActive(false);
                    return go;
                }
            }

            return null;
        }
    }
}
