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
        public static UnityAction OnZombieChangeStateUp;
        public static UnityAction OnZombieChangeStateDown;
        public static UnityAction OnZombieSearchState;

        private int level = 0;

        private void Start()
        {
            level = 0;
            ResetUI();
        }

        private void OnEnable()
        {
            OnZombieChangeStateUp += ChangeStateUp;
            OnZombieChangeStateDown += ChangeStateDown;
            OnZombieSearchState += SearchState;
        }

        private void OnDisable()
        {
            OnZombieChangeStateUp -= ChangeStateUp;
            OnZombieChangeStateDown -= ChangeStateDown;
            OnZombieSearchState -= SearchState;
        }

        private void SearchState()
        {
            for (int i = 0; i < circlesImages.Length; i++)
            {
                circlesImages[i].enabled = false;
            }
            bigEye.sprite = searchEye;
        }

        private void ChangeStateUp()
        {
            circlesImages[level].sprite = fullCircle; 
            level++;
        }

        private void ChangeStateDown()
        {
            circlesImages[level].sprite = emptyCircle;
            level--;
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
