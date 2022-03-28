using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class CompanionController : MonoBehaviour
    {
        [SerializeField] private InputHandler input;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();       
        }

        private void OnEnable()
        {
            input.EnableCompanionInput();

            //input.companionMove 
            //input.companionLook
        }

        private void OnDisable()
        {
            //input.companionMove 
            //input.companionLook  
        }

        private void Move()
        {
            //rb.AddForce()
        }
    }
}
