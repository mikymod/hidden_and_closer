using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HNC
{
    public class LightingUI : MonoBehaviour
    {
        [SerializeField] private Image bulbImage;
        public static UnityAction<bool> OnLighting;

        //private void Start()
        //{
        //    bulbImage.enabled = false;
        //}

        private void OnEnable()
        {
            OnLighting += IsInLight;
            CompanionController.OnCompanionControlStarted += DisableUILight;
            CompanionController.OnCompanionControlFinish += DisableUILight;

        }

        private void OnDisable()
        {
            OnLighting -= IsInLight;
            CompanionController.OnCompanionControlStarted -= DisableUILight;
            CompanionController.OnCompanionControlFinish -= DisableUILight;
        }

        private void IsInLight(bool isIlluminated)
        {
            bulbImage.enabled = isIlluminated;
        }

        private void DisableUILight() 
        {
            bulbImage.enabled = false;
        }
    }
}
