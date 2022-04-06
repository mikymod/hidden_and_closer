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
        
        [SerializeField] private Sprite idleEyeSprite;
        [SerializeField] private Sprite searchEyeSprite;
        [SerializeField] private Sprite attackEyeSprite;
        [SerializeField] private Sprite emptyCircleSprite;
        [SerializeField] private Sprite fullCircleSprite;


        [SerializeField] private Image bigEye;
        [SerializeField] private Image[] circlesImages;

        public UnityAction<EnemyFSMState> OnZombieChangeState;

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

        private void ChangeState(EnemyFSMState currentState)
        {
            if (currentState == EnemyFSMState.Idle)
            {
                ResetUI();
            }
            else if (currentState == EnemyFSMState.Search)
            {
                bigEye.sprite = searchEyeSprite;
                for (int i = 0; i < circlesImages.Length; i++)
                {
                    circlesImages[i].enabled = false;
                }
            }
            else if (currentState == EnemyFSMState.Death)
            {
                bigEye.enabled = false;
                for (int i = 0; i < circlesImages.Length; i++)
                {
                    circlesImages[i].enabled = false;
                }
            }
            else
            {
                if (currentState == EnemyFSMState.Attack)
                {
                    bigEye.sprite = attackEyeSprite;
                }
                else
                {
                    bigEye.sprite = idleEyeSprite;
                }
                for (int i = 0; i < (int)currentState; i++)
                {
                    circlesImages[i].enabled = true;
                    circlesImages[i].sprite = fullCircleSprite;
                }
            }
        }

        private void ResetUI() 
        {
            bigEye.sprite = idleEyeSprite;
            for (int i = 0; i < circlesImages.Length; i++)
            {
                circlesImages[i].enabled = true;
                circlesImages[i].sprite = emptyCircleSprite;
            }
        }
    }
}
