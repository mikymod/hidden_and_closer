using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HNC
{
    public class LightingUI : MonoBehaviour
    {
        private Image bulbImage;
        public static UnityAction<bool> OnLighting;

        private void Awake()
        {
            bulbImage = GetComponentInChildren<Image>();
        }

        private void Start()
        {
            bulbImage.enabled = false;
        }

        private void OnEnable()
        {
            OnLighting += IsInLight;
        }

        private void OnDisable()
        {
            OnLighting -= IsInLight;
        }

        private void IsInLight(bool light)
        {
            bulbImage.enabled = light;       
        }
    }
}
