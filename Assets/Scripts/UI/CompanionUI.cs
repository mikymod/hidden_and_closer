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
        private Image carImage;


        private void Awake()
        {
            carImage = GetComponentInChildren<Image>();    
        }

        private void Start()
        {
            carImage.sprite = car;
        }

        private void OnEnable()
        {
            //TODO event car destroy
            PlayerController.CompanionControl += IsControllingCar;
        }

        private void OnDisable()
        {
            //TODO event car destroy
            PlayerController.CompanionControl -= IsControllingCar;
        }

        private void IsControllingCar(Transform transform)
        {
            carImage.enabled = !carImage.enabled;
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
