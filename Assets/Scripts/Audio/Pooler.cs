using System.Collections.Generic;
using UnityEngine;

namespace HNC {
    public class Pooler : MonoBehaviour {
        [SerializeField] private GameObject soundEmitter;
        [SerializeField] private int soundEmittersCount;
        private List<GameObject> soundEmitters;

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
                    return soundEmitters[i];
                }
            }

            GameObject go = Instantiate(soundEmitter, transform);
            go.SetActive(true);
            soundEmitters.Add(go);


            return go;
        }

        public GameObject DisposeSoundEmitter(AudioClipsBankSO audioClipBank)
        {
            for (int i = 0; i < soundEmitters.Count; i++)
            {
                if (soundEmitters[i].activeInHierarchy && soundEmitters[i].GetComponent<AudioSource>().clip == audioClipBank.GetClip())
                {    
                    GameObject go = soundEmitters[i];
                    return go;
                }
            }

            return null;
        }
    }
}
