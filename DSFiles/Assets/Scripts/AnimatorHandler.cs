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
        }

        public void UpdateAnimatorValues(float vertMovement, float horizonMovement)
        {
            #region Vertical
            float v = 0;

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
            animator.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
            //Debug.Log("Trying to set Vertical (" + vertical + ") to " + v);
            //Debug.Log("Actual Vertical value: " + animator.GetFloat(vertical));

            #region Horizontal
            float h = 0;

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
                Debug.Log("rootmotion is " + animator.applyRootMotion);
            }
            animator.SetBool("isInteracting", isInteracting);
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

        private void OnAnimatorMove()//when animator moves something
        {
            if (inputHandler.isInteracting == false)
            {
                return;
            }

            float delta = Time.deltaTime;
            //playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPos = animator.deltaPosition;
            deltaPos.y = 0;

            Vector3 velocity = deltaPos / delta;
            playerLocomotion.rigidbody.velocity = velocity;
        }

    }
}
