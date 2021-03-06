using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace agahan_vivas
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator animator;
        public InputHandler inputHandler;
        public PlayerLocomotion playerLocomotion;
        public CapsuleCollider capsule;
        int vertical;
        int horizontal;
        public bool canRotate;
        
        public void Initialize() 
        {
            animator = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            capsule = GetComponentInParent<CapsuleCollider>();
        }

        public void UpdateAnimatorValues(float vertMovement, float horizonMovement, bool isSprinting)
        {
            float v = 0;
            float h = 0;
            #region Vertical

            //clamping the values
            if (vertMovement > 0 && vertMovement <= 0.55f) 
            {
                v = 0.5f;
            } else if (vertMovement > 0.55f)
            {
                v = 1;
            } else if (vertMovement < 0 && vertMovement >= -0.55f)
            {
                v = -0.5f;
            } else if (vertMovement < -0.55f)
            {
                v = -1;
            } else
            {
                v = 0;
            }
            #endregion

            if (isSprinting && inputHandler.moveAmount > 0)
            {
                v = 2;
                h = horizonMovement;
            }

            animator.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
            //Debug.Log("Trying to set Vertical (" + vertical + ") to " + v);
            //Debug.Log("Actual Vertical value: " + animator.GetFloat(vertical));

            #region Horizontal

            if (horizonMovement > 0 && horizonMovement <= 0.55f)
            {
                h = 0.5f;
            }
            else if (horizonMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizonMovement < 0 && horizonMovement >= -0.55f)
            {
                h = -0.5f;
            }
            else if (horizonMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion
            animator.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);
            //Debug.Log("Trying to set Horizontal (" + vertical + ") to " + v);
            //Debug.Log("Actual Vertical value: " + animator.GetFloat(horizontal));
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            //isInteracting = True if the player is playing an animation that, by design, cannot be cancelled by rolling
            animator.applyRootMotion = isInteracting;
            if (animator.applyRootMotion)
            {
                //Debug.Log("rootmotion is " + animator.applyRootMotion);
            }
            animator.SetBool("isInteracting", isInteracting);
            EnableIsInvulnerable();
            animator.CrossFade(targetAnim, 0.2f);
        }

        public void CanRotate()
        {
            canRotate = true; //this will be set in the unity menu
        }

        public void StopRotate()
        {
            canRotate = false;
        }

        public void EnableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }

        public void DisableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", false);
        }

    }
}
