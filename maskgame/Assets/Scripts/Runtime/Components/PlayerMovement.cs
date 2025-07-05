using System;
using UnityEngine;

namespace Runtime.Components
{
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController _characterController;

        private Vector2 movementInput;
        private float verticalVelocityValue = 0;

        private int jumpsLeft;
        private bool isSprinting = false;

        [Header("Movement parameters")]
        [SerializeField] private float speed = 4f;
        [SerializeField] private float sprintSpeed = 8f;
        [SerializeField] private float acceleration = 5f;

        [Header("Jump Parameters")]
        [SerializeField] private float jumpHeight = 10;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private float jumpStopThreshold = 0.2f;
        [SerializeField] private int maxJumps = 2;

        [field: Header("Multipliers")]
        [field: SerializeField] public float SpeedMultiplier { get; private set; } = 1;
        [field: SerializeField] public float GravityMultiplier { get; private set; } = 1;

        public Vector3 Speed => _characterController.velocity;
        public bool IsGrounded => _characterController.isGrounded;


        void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        void FixedUpdate()
        {
            verticalVelocityValue = ApplyGravity(verticalVelocityValue, Time.fixedDeltaTime);

            var velocity = GetVelocity();
            _characterController.Move(velocity * Time.fixedDeltaTime);

            if (IsGrounded)
            {
                jumpsLeft = maxJumps;
                verticalVelocityValue = _characterController.velocity.y;
            }
        }


        public void PerformJump()
        {
            if (jumpsLeft > 0)
            {
                verticalVelocityValue = Mathf.Sqrt(jumpHeight * -2 * gravityValue * GravityMultiplier);
                jumpsLeft--;
            }
        }

        public void StopPerformJump()
        {
            if (IsGrounded || verticalVelocityValue < jumpStopThreshold)
                return;

            verticalVelocityValue = jumpStopThreshold;
        }


        public void SetMovementInput(Vector2 value)
        {
            movementInput = value;
        }

        public void ToggleSprint(bool value)
        {
            if (isSprinting == value)
                return;

            isSprinting = value;
        }


        public void SetSpeedMultiplier(float value)
        {
            SpeedMultiplier = value;
        }

        public void SetGravityMultiplier(float value)
        {
            GravityMultiplier = value;
        }


        /// <summary>
        /// Get full velocity for character controller
        /// </summary>
        private Vector3 GetVelocity()
        {
            var targetSpeed = GetHorizontalSpeed();
            var inputVelocity = GetInputVelocity(targetSpeed);

            var velocityHorizontal = Vector3.MoveTowards(Speed, inputVelocity, acceleration);
            velocityHorizontal.y = 0;

            var velocityVertical = verticalVelocityValue * Vector3.up;

            return velocityHorizontal + velocityVertical;
        }

        /// <summary>
        /// Get desired horizontal velocity based on input
        /// </summary>
        private Vector3 GetInputVelocity(float speed)
        {
            var inputWorld = (movementInput.x * transform.forward + movementInput.y * transform.right).normalized;

            return inputWorld * speed;
        }

        /// <summary>
        /// Get desired horizontal speed
        /// </summary>
        private float GetHorizontalSpeed()
        {
            if (isSprinting)
                return sprintSpeed * SpeedMultiplier;
            else
                return speed * SpeedMultiplier;
        }

        /// <summary>
        /// Apply gravity to provided vertical speed
        /// </summary>
        private float ApplyGravity(float verticalSpeed, float deltaTime)
        {
            return verticalSpeed + gravityValue * GravityMultiplier * deltaTime;
        }
    }
}
