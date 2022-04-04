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
        
        //public static UnityAction<EnemyFSMState> OnZombieChangeState;

        private void Start()
        {
            ResetUI();
        }

        private void OnEnable()
        {
            //OnZombieChangeState += ChangeState;
        }

        private void OnDisable()
        {
            //OnZombieChangeState -= ChangeState;
        }

        //private void ChangeState(EnemyFSMState currentState)
        //{
        //    switch (currentState)
        //    {
        //        case EnemyFSMState.Idle:
        //            ResetUI();
        //            break;
        //        case EnemyFSMState.Suspicious:
        //            bigEye.sprite = idleEyeSprite;
        //            for (int i = 0; i < 0; i++)
        //            {
        //                circlesImages[i].enabled = true;
        //                circlesImages[i].sprite = fullCircleSprite;
        //            }
        //            break;
        //        case EnemyFSMState.Alert:
        //            bigEye.sprite = idleEyeSprite;
        //            for (int i = 0; i < 1; i++)
        //            {
        //                circlesImages[i].enabled = true;
        //                circlesImages[i].sprite = fullCircleSprite;
        //            }
        //            break;
        //        case EnemyFSMState.Attack:
        //            bigEye.sprite = attackEyeSprite;
        //            for (int i = 0; i < circlesImages.Length; i++)
        //            {
        //                circlesImages[i].enabled = true;
        //                circlesImages[i].sprite = fullCircleSprite;
        //            }
        //            break;
        //        case EnemyFSMState.Search:
        //            bigEye.sprite = searchEyeSprite;
        //            for (int i = 0; i < circlesImages.Length; i++)
        //            {
        //                circlesImages[i].enabled = false;
        //            }
        //            break;
        //        case EnemyFSMState.Death:
        //            bigEye.enabled = false;
        //            for (int i = 0; i < circlesImages.Length; i++)
        //            {
        //                circlesImages[i].enabled = false;
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}

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
