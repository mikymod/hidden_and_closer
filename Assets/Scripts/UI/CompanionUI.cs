using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HNC
{
    public class CompanionUI : MonoBehaviour
    {
        [SerializeField] private Sprite car;
        [SerializeField] private Sprite carBlackAndWhite;
        [SerializeField] private Image carImage;

        private void Start()
        {
            carImage.enabled = true;
            carImage.sprite = car;
        }

        private void OnEnable()
        {
            //TODO event car destroy
            CompanionController.OnCompanionControlStarted += EnableDisableUILight;
            CompanionController.OnCompanionControlFinish += EnableDisableUILight;
            CompanionController.OnCompanionDestroy += CarDestroy;
            CompanionController.OnCompanionRestore += CarRestored;
        }

        private void OnDisable()
        {
            //TODO event car destroy
            CompanionController.OnCompanionControlStarted -= EnableDisableUILight;
            CompanionController.OnCompanionControlFinish -= EnableDisableUILight;
            CompanionController.OnCompanionDestroy -= CarDestroy;
            CompanionController.OnCompanionRestore -= CarRestored;
        }

        private void EnableDisableUILight()
        {
            carImage.enabled = !carImage.enabled;
        }

        private void CarDestroy() {
            ChangeCarImage(false);
        }

        private void CarRestored() {
            ChangeCarImage(true);
        }

        private void ChangeCarImage(bool isCarUsable)
        {
            if (isCarUsable)
            {
                carImage.sprite = car;
            }
            else
            {
                carImage.sprite = carBlackAndWhite;
            }
        }
    }
}

