using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace agahan_vivas
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;

        public bool isInvulnerable;

        void Start()
        {
            inputHandler = GetComponent<InputHandler>();    
            anim = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            inputHandler.isInteracting = anim.GetBool("isInteracting");
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;

            //i don't know if this actually does anything aside from showing na invulnerable yung character at the current frame
            isInvulnerable = anim.GetBool("isInvulnerable");
        }
    }
}