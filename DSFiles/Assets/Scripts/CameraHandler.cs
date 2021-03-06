using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace agahan_vivas
{
    public class CameraHandler : MonoBehaviour
    {
        #region Variables
        public Transform targetTransform; //transform that camera will eventually go to; aka player's Transform/location
        public Transform cameraTransform; //current Transform of camera
        public Transform cameraPivotTransform; //transform of camera's pivot, which is how the camera will turn
        private Transform myTransform; //for transforming the gameobjects' transforms
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minPivot = -35;
        public float maxPivot = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minCollisionOffset = 0.2f;
        #endregion

        private void Awake()
        {
            Debug.Log("setting camerahandler...");
            singleton = this;
            Debug.Log("done!");
            //set myTransform to the transform of the gameobject attached
            myTransform = transform;
            //set default position to camera transform's pos relative to "transform"
            defaultPosition = cameraTransform.localPosition.z; 
            //this is for the collision of the camera (to be discussed in video 3 of the tutorial
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public void FollowTarget (float delta)
        {
            //make targetPosition = the vector that allows myTransform to move to targetTransform at a rate of delta/followspeed
            Vector3 targetPosition = Vector3.SmoothDamp
                (myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);

            //move myTransform using targetPosition
            //(this will be called on update, so this will let the camera follow the player)
            myTransform.position = targetPosition;
        }

        public void HandleCamRotation (float delta, float mouseXInput, float mouseYInput)
        {
            float mouseX = mouseXInput * lookSpeed;
            float mouseY = mouseYInput * pivotSpeed;

            //Debug.Log("mouseX with mods:" + mouseX);
            //Debug.Log("mouseY with mods:" + mouseY);

            lookAngle += (mouseX / delta);
            pivotAngle -= (mouseY / delta);

            //makes sure that mouse look only goes up to a certain point
            //(or else you could literally flip the camera)
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            Vector3 rotation = Vector3.zero;

            //set lookAngle to y because we're going to rotate around the y-axis
            rotation.y = lookAngle;

            //use Euler to add rotation of (lookAngle) degrees around
            //the y axis (see Euler documentation tooltip)
            Quaternion targetRotation = Quaternion.Euler(rotation);

            //rotate the camera by setting
            myTransform.rotation = targetRotation;

            //do the same thing for pivot angle by rotating around the x-axis
            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            //add (pivotAngle) degrees of rotation around x-axis to targetRotation
            targetRotation = Quaternion.Euler(rotation);

            //rotate the camera by setting
            cameraPivotTransform.localRotation = targetRotation;

        }

        public void HandleCameraCollisions (float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();


            //parameters for SphereCast are:
            //Vector3 origin,
            //float radius,
            //Vector3 direction,
            //out RaycastHit hitInfo,
            //float maxDistance,
            //int layerMask

            //makes a sphere around camera that returns true if it hits a collider
            if (Physics.SphereCast(cameraPivotTransform.position, 
                cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(distance - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minCollisionOffset)
            {
                targetPosition = -minCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }
    }
}

