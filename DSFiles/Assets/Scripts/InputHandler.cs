using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace agahan_vivas
{
    public class InputHandler : MonoBehaviour
    {
        #region Variables
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool rollFlag;
        public bool isInteracting; //is reset after roll animation is done
                                   //(check resetisinteracting behavior script in Roll state)
        public float rollInputTimer;
        public bool sprintFlag;


        PlayerControls inputActions;
        CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;
        #endregion

        private void Awake()
        {
            Debug.Log("loading cameraHandler.");
            cameraHandler = CameraHandler.singleton;
            if (cameraHandler != null)
            {
                Debug.Log("success!");
            }
        }
        
        private void FixedUpdate()
        {
            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCamRotation(delta, mouseX, mouseY);
            }
            else
            {
                Debug.Log("cameraHandler is null!");
            }
        }

        public void OnEnable() 
        {
            if (inputActions == null) //if there are no player control assets referenced, make one now
            {
                inputActions = new PlayerControls();

                #region Adding Listeners
                #region Sources for Understanding Event Listeners and Event Lambdas (Expand this)
                /* https://www.youtube.com/watch?v=FvNac5ozfds at time 6:52 for explanation, 8:56 for action delegate example
                 * https://www.youtube.com/watch?v=m5WsmlEOFiA at 15:20 for subscribing to events, 23:25 for usage of lambdas
                 * 
                 * We're adding an event listener here, which is same as saying 
                 * we're subscribing to / keeping track of an event. 
                 * 
                 * Here, event is when movement is performed, and we're calling a function (in this case the lambda expression 
                 * where inputActions is input and movementInput is output) when it happens. 
                 * 
                 * functionally same as this:
                 * inputActions.PlayerMovement.Movement.performed += updateMovement; 
                 * 
                 * but we would need to create an updateMovement function for this 
                 * that takes an InputAction.CallbackContext as a parameter
                 * 
                 * updateMovement(InputAction.CallbackContext context){
                 *      foo
                 * }
                 */
                #endregion

                //read the movement value when WASD keys are pressed, then set movementInput to that
                inputActions.PlayerMovement.Movement.performed += onMove;
                //read the movement value when mouse is moved, then set cameraInput to that
                inputActions.PlayerMovement.Camera.performed += onMouseMove;
                #endregion
            }

            inputActions.Enable();
        }

        public void onMove(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        public void onMouseMove(InputAction.CallbackContext context)
        {
            cameraInput = context.ReadValue<Vector2>();
        }

        private void OnDisable()
        {
            inputActions.Disable();

            //unsubscribing to events to prevent memory leaks
            inputActions.PlayerMovement.Movement.performed -= inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed -= i => cameraInput = i.ReadValue<Vector2>();
        }

        /// <summary>
        /// Currently a wrapper method for moveInput. Called in the Update method of the Locomotion script.
        /// </summary>
        /// <param name="delta">Time delta.</param>
        public void TickInput(float delta)
        {
            moveInput(delta);
            HandleRollInput(delta);
        }

        /// <summary>
        /// Facilitates preprocessing of movement data received from input, aimed at 
        /// normalizing the movement data vectors to make sure movement is 
        /// constant unless acted upon by player (Sprinting, etc).
        /// </summary>
        /// <param name="delta">The time delta.</param>
        public void moveInput(float delta) 
        { 
            //read the x and y components of the movement values for WASD and set to horizontal, vertical
            horizontal = movementInput.x;
            vertical = movementInput.y;

            //clamp values between 0 and 1 for normalization (and animation) purposes
            //Clamp01 means all values are "clamped" to a min of 0f and max of 1f (f's are units of force in unity)
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            //read the x and y components of the movement values for the mouse/right joystick and set to mouseX, mouseY
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput (float delta)
        {
            //check if roll has been
            b_Input = (inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed);
            if (b_Input) //if roll button is pressed
            {
                rollInputTimer += delta; //count time button is pressed
                sprintFlag = true; //sprint while button is held
            }
            else {
                //sprintFlag = false;
                if (rollInputTimer > 0 && rollInputTimer < 0.5f) //check to see if player simply stopped sprinting or wanted to roll
                {
                    sprintFlag = false;
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
            //Debug.Log("Roll.phase = " + inputActions.PlayerActions.Roll.phase + "\nb_input = " + b_Input + "\nrollFlag = " + rollFlag);
        }
    }
}

