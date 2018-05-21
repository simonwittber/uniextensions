using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.UI
{
    [AddComponentMenu ("Camera-Control/Smooth Mouse Look")]
    public class SmoothMouseLook : MonoBehaviour
    {
        
        public enum RotationAxes
        {
            MouseXAndY = 0,
            MouseX = 1,
            MouseY = 2

        }

        public RotationAxes axes = RotationAxes.MouseXAndY;

        public float sensitivityX = 15F;
        public float sensitivityY = 15F;
        public float damping = 10;
        public float minimumX = -360F;
        public float maximumX = 360F;
        
        public float minimumY = -60F;
        public float maximumY = 60F;
        
        float rotationX = 0F;
        float rotationY = 0F;

        float _rotationX = 0F;
        float _rotationY = 0F;
        


        Quaternion originalRotation;

        void Update ()
        {

            rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
            rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
            _rotationX = Mathf.Lerp (_rotationX, rotationX, Time.deltaTime * damping);
            _rotationY = Mathf.Lerp (_rotationY, rotationY, Time.deltaTime * damping);


            var yQuaternion = Quaternion.AngleAxis (_rotationY, Vector3.left);
            var xQuaternion = Quaternion.AngleAxis (_rotationX, Vector3.up);

            if (axes == RotationAxes.MouseX)
                transform.localRotation = originalRotation * xQuaternion;
            if (axes == RotationAxes.MouseY)
                transform.localRotation = originalRotation * yQuaternion;
            if (axes == RotationAxes.MouseXAndY)
                transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        
        

        }

        void Start ()
        {           
            if (GetComponent<Rigidbody> ())
                GetComponent<Rigidbody> ().freezeRotation = true;
            originalRotation = transform.localRotation;
        }

        public static float ClampAngle (float angle, float min, float max)
        {
            angle = angle % 360;
            if ((angle >= -360F) && (angle <= 360F)) {
                if (angle < -360F) {
                    angle += 360F;
                }
                if (angle > 360F) {
                    angle -= 360F;
                }           
            }
            return Mathf.Clamp (angle, min, max);
        }
    }
}
