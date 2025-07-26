using System;
using UnityEngine;

namespace Runtime.Entities.Player
{
    public class PlayerMovementComponent : MonoBehaviour, IPlayerMovementInput
    {
        private const float VERTICAL_SPEED_WHEN_GROUNDED = -1;

        private CharacterController _characterController;
        private IPlayerAnimationsMovementControl _movementAnimations;

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
        private Vector2 InputSpeed => new Vector2(movementInput.y, movementInput.x);
        public bool IsGrounded => _characterController.isGrounded;


        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _movementAnimations = GetComponent<IPlayerAnimationsMovementControl>();
        }

        void Update()
        {
            verticalVelocityValue = ApplyGravity(verticalVelocityValue, Time.deltaTime);

            var velocity = GetVelocity();
            _characterController.Move(velocity * Time.deltaTime);

            _movementAnimations.SetRelativeSpeed(InputSpeed.x, InputSpeed.y);
            _movementAnimations.ToggleAirborne(!IsGrounded);

            if (IsGrounded)
            {
                jumpsLeft = maxJumps;
                verticalVelocityValue = VERTICAL_SPEED_WHEN_GROUNDED;
            }
        }


        public void SetMovementInput(Vector2 value)
        {
            movementInput = value;
        }


        public void SetJumpPressed()
        {
            if (jumpsLeft > 0)
            {
                verticalVelocityValue = Mathf.Sqrt(jumpHeight * -2 * gravityValue * GravityMultiplier);
                jumpsLeft--;

                _movementAnimations.TriggerJump();
            }
        }

        public void SetJumpReleased()
        {
            if (IsGrounded || verticalVelocityValue < jumpStopThreshold)
                return;

            verticalVelocityValue = jumpStopThreshold;
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
            var inputWorld = (InputSpeed.x * transform.forward + InputSpeed.y * transform.right).normalized;

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
