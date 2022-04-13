using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class Pooler : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int poolCount;
        private List<GameObject> _objects;

        private void Awake()
        {
            _objects = new List<GameObject>();
            for (int i = 0; i < poolCount; i++)
            {
                GameObject go = Instantiate(prefab, transform);
                go.SetActive(false);
                _objects.Add(go);
            }
        }

        public GameObject Get()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                if (!_objects[i].activeInHierarchy)
                {
                    _objects[i].SetActive(true);
                    return _objects[i];
                }
            }

            GameObject go = Instantiate(prefab, transform);
            go.SetActive(true);
            _objects.Add(go);

            return go;
        }

        // FIXME: wrong place. Pooler should manage every kind of object
        public GameObject DisposeSoundEmitter(AudioClipsBankSO audioClipBank)
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i].activeInHierarchy && _objects[i].GetComponent<AudioSource>().clip == audioClipBank.GetClip())
                {
                    GameObject go = _objects[i];
                    return go;
                }
            }

            return null;
        }

        public bool IsPlaying(AudioClipsBankSO audioClipBank)
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i].activeInHierarchy && _objects[i].GetComponent<AudioSource>().clip == audioClipBank.GetClip())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
