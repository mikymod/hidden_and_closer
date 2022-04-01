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

        public static UnityAction<bool> OnChangeCarImage;
        public static UnityAction<bool> OnControllingCar;

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
            OnChangeCarImage += ChangeCarImage;
            OnControllingCar += IsControllingCar;
        }

        private void OnDisable()
        {
            OnChangeCarImage -= ChangeCarImage;
            OnControllingCar -= IsControllingCar;
        }

        private void IsControllingCar(bool control)
        {
            carImage.enabled = control; 
        } 

        private void ChangeCarImage(bool isCarUsable)
        {
            if (isCarUsable)
            {
                carImage.sprite = carBlackAndWhite;
            }
            else
            {
                carImage.sprite = car;
            }
        }
    }
}
