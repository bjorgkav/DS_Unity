using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace agahan_vivas
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator animator;
        int vertical;
        int horizontal;
        public bool canRotate;
        
        public void Initialize() 
        {
            animator = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
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
            animator.applyRootMotion = isInteracting;
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
    }
}
