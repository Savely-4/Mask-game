using UnityEngine;

namespace Runtime.Entities.Player
{
    public class PlayerCameraComponent : MonoBehaviour, IPlayerCameraInput
    {
        [Header("Rotation Transforms")]
        public Transform horizontal;
        public Transform vertical;

        [Header("Settings")]
        public float sensitivity = 2.0f;
        public float minYAngle = -30f;
        public float maxYAngle = 60f;

        private float currentY = 0f;

        private Vector2 input;


        void LateUpdate()
        {
            var inputX = input.x * sensitivity;
            var inputY = -input.y * sensitivity;

            currentY += inputY;
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

            horizontal.forward = Quaternion.AngleAxis(inputX, Vector3.up) * horizontal.forward;
            vertical.localRotation = Quaternion.Euler(currentY, 0, 0);
        }


        public void SetCameraInput(Vector2 value)
        {
            input = value;
        }
    }
}