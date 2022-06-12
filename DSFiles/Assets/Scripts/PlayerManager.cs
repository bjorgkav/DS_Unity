using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace agahan_vivas
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;
        CapsuleCollider capsuleCollider;
        SphereCollider sphereCollider;

        public bool isInvulnerable;

        void Start()
        {
            inputHandler = GetComponent<InputHandler>();    
            anim = GetComponentInChildren<Animator>();
            capsuleCollider = GetComponentInChildren<CapsuleCollider>();
            sphereCollider = GetComponentInChildren<SphereCollider>();
        }

        void Update()
        {
            inputHandler.isInteracting = anim.GetBool("isInteracting");
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;

            //i don't know if this actually does anything aside from showing na invulnerable yung character at the current frame
            isInvulnerable = anim.GetBool("isInvulnerable");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
            {
                if (!isInvulnerable)
                {
                    Debug.Log("Ouch! damage taken.");
                }
                else
                {
                    Debug.Log("Damage avoided!");
                }
            }
        }

        /*
        private void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                print(contact.otherCollider.isTrigger);
                if (!isInvulnerable && contact.otherCollider.isTrigger && contact.thisCollider == capsuleCollider)
                {
                    Debug.Log("OUCH! That hurts!");
                }
                else
                {
                    //Debug.Log("Either invulnerable or not working right.");
                }
            }
        }
        */
    }
}