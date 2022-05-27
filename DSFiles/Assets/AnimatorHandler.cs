using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace agahan_vivas
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator anim;
        int vertical;
        int horizontal;
        public bool canRotate;
        
        public void Initialize() 
        { 
            anim = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float vertMovement, float horizonMovement)
        {
            #region Vertical
            float v = 0;

            //clamping the values
            if (vertMovement < 0 && vertMovement < 0.55f) 
            {
                v = 0.5f;
            } else if (vertMovement > 0.55f)
            {
                v = 1;
            } else if (vertMovement < 0 && vertMovement > -0.55f)
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

            #region Horizontal
            float h = 0;

            if (horizonMovement < 0 && horizonMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizonMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizonMovement < 0 && horizonMovement > -0.55f)
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

            anim.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
            anim.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);
        }
    }
}
