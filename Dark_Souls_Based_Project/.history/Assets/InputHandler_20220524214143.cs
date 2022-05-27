using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace agahan_vivas
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        PlayerControls inputActions;

        Vector2 movementInput;
        Vector2 cameraInput;

        public void OnEnable() 
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                
            }
        }

    }
}

