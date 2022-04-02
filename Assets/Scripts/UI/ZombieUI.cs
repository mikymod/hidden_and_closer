using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HNC
{
    public class ZombieUI : MonoBehaviour
    {
        
        [SerializeField] private Sprite idleEye;
        [SerializeField] private Sprite searchEye;
        [SerializeField] private Image bigEye;
        [SerializeField] private Image[] circlesImages; 
        [SerializeField] private Sprite emptyCircle;
        [SerializeField] private Sprite fullCircle;
        public static UnityAction OnZombieChangeState;

        private void Start()
        {
            ResetUI();
        }

        private void OnEnable()
        {
            OnZombieChangeState += ChangeState;
        }

        private void OnDisable()
        {
            OnZombieChangeState -= ChangeState;
        }

        private void ChangeState()
        {
            
        }

        private void ResetUI() 
        {
            bigEye.sprite = idleEye;
            for (int i = 0; i < circlesImages.Length; i++)
            {
                circlesImages[i].enabled = true;
                circlesImages[i].sprite = emptyCircle;
            }
        }
    }
}
