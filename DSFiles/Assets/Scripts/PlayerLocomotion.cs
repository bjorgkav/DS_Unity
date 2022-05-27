using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace agahan_vivas 
{
    public class PlayerLocomotion : MonoBehaviour
    {
        //initializing
        Transform cameraObject;
        InputHandler inputHandler;
        Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform; // prevent this transform object from showing up in the unity menus
                                      // to lessen clutter while allowing public access across classes
        [HideInInspector]
        public AnimatorHandler animHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCam;

        public CharacterController characterController;

        [Header("Stats")] //adds custom section in the unity inspector (the right-hand window)
        [SerializeField]  //to customize these fields from unity itself
        float movementSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>(); //get rigidbody of gameobject script is used on

            inputHandler = GetComponent<InputHandler>();

            animHandler = GetComponentInChildren<AnimatorHandler>(); //in children because animatorhandler will be under player model

            myTransform = transform; //set myTransform to the current gameobject (likely player model)'s Transform
                                     //property (position, rotation, scale).

            cameraObject = Camera.main.transform;//set cameraObject to the the Main camera
                                                 //(which is also the Player's camera in this case)
            
            characterController = GetComponent<CharacterController>();

            animHandler.Initialize();
        }

        public void Update()
        {
            float delta = Time.deltaTime;

            //call input processing methods
            inputHandler.TickInput(delta);

            //camera-relative direction * inputted direction value (this will be negative if opposite direction wanted)
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.y = 0;

            //makes sure net movement value before movement speed
            //is always 1 (allows for more consistent speed in all directions)
            moveDirection.Normalize();

            //apply speed multiplier
            float speed = movementSpeed;
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);

            //after calculating the movement vectors, set that to be the rigidbody (character)'s velocity to start movement.
            rigidbody.velocity = projectedVelocity;

            //call HandleRotation to make the character face the way they're walking
            if(animHandler.canRotate)
            {
                HandleRotation(delta);
            }

            animHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);
        }

        #region Movement (expand this)
        Vector3 normalVector; //default values are normal vector values
        Vector3 targetPositioning;

        /// <summary>
        /// Method for making sure the character is facing the way they're moving.
        /// </summary>
        /// <param name="delta">The time delta.</param>
        private void HandleRotation(float delta)
        {
            //initialize
            Vector3 targetDirection = Vector3.zero;
            float movementOverride = inputHandler.moveAmount;

            //camera-relative direction * inputted direction value (this will be negative if opposite direction wanted)
            targetDirection = cameraObject.forward * inputHandler.vertical;
            targetDirection += cameraObject.right * inputHandler.horizontal;

            //normalize for consistency before rotation speed multiplier
            targetDirection.Normalize();

            //remove y bwcause we only need x for horizontal rotations.
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero) //if no movement, don't rotate
            {
                targetDirection = myTransform.forward;
            }

            float rs = rotationSpeed;

            //Quaternions are used to represent rotations in Unity (mouseover for further descriptions)
            //LookRotation creates a rotation from the given Vector3
            //Slerp creates rotations between the first parameter and the second parameter
            //(it "animates" the rotation from one config to another)
            Quaternion tr = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            //rotate character
            myTransform.rotation = targetRotation;

        }
        #endregion

    }
}

