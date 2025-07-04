using UnityEngine;

namespace Runtime.Components
{
    
    //TODO: Add state machine on top
    public class PlayerMovement : MonoBehaviour
    {

        private CharacterController _characterController;
        private Vector2 _movementInput;
        private float verticalVelocityValue = 0;
        private int jumpsLeft;

        /// <summary>
        /// Time for which movement input was remaining unchanged
        /// </summary>
        private float timeSinceInputChanged;

        [Header("Movement parameters")]
        [SerializeField] private float speed = 4f;
        [SerializeField] private float sprintSpeed = 8f;
        [SerializeField] private float acceleration = 5f;

        [Header("Jump Parameters")]
        [SerializeField] private float jumpHeight = 10;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private float jumpStopThreshold = 0.2f;
        [SerializeField] private int maxJumps = 2;

        //TODO: Add ability to change multipliers
        [field: Header("Multipliers")]
        [field: SerializeField] public float SpeedMultiplier { get; private set; } = 1;
        [field: SerializeField] public float GravityMultiplier { get; private set; } = 1;

        private bool _isSprinting = false;
        public Vector3 Speed => _characterController.velocity;
        public bool IsGrounded => _characterController.isGrounded;

        //TODO: Add support for different movement modes
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        //TODO: Use FixedUpdate
        // Update is called once per frame
        void FixedUpdate()
        {
            timeSinceInputChanged += Time.fixedDeltaTime;

            verticalVelocityValue = ApplyGravity(verticalVelocityValue, Time.fixedDeltaTime);

            var velocity = GetVelocity();

            _characterController.Move(velocity * Time.fixedDeltaTime);

            if (IsGrounded)
                verticalVelocityValue = _characterController.velocity.y;
        }

        public void ResetJumps()
        {
            if(IsGrounded)
                jumpsLeft = maxJumps;
        } 
        
        public void PerformJump()
        {
            if (jumpsLeft > 0)
            {
                Jump();
                jumpsLeft--;
            }
        }

        public void StopPerformJump()
        {
            StopJump();
        }
        
        public void SetMovementInput(Vector2 value)
        {
            _movementInput = value;
            timeSinceInputChanged = 0;
        }
        
        public void ToggleSprint()
        {
            _isSprinting = true;
            timeSinceInputChanged = 0;
        }
        
        public void UnToggleSprint()
        {
            _isSprinting = false;
            timeSinceInputChanged = 0;
        }

        private void Jump()
        {
            verticalVelocityValue = Mathf.Sqrt(jumpHeight * -2 * gravityValue * GravityMultiplier);
            timeSinceInputChanged = 0;
        }
        
        private void StopJump()
        {
            if (IsGrounded || verticalVelocityValue < jumpStopThreshold)
                return;

            verticalVelocityValue = jumpStopThreshold;
            timeSinceInputChanged = 0;
        }

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
            var inputWorld = (_movementInput.x * transform.forward + _movementInput.y * transform.right).normalized;

            return speed * inputWorld;
        }
        
        private float GetHorizontalSpeed()
        {
            if (_isSprinting)
                return sprintSpeed * SpeedMultiplier;
            else
                return speed * SpeedMultiplier;
        }

        //TODO: Replace arguments with animation curves
        private float GetLerpValueFromTime(float currentTime, float duration)
        {
            return Mathf.Clamp(currentTime / duration, 0, 1);
        }

        /// <summary>
        /// Apply gravity to provided vertical speed
        /// </summary>
        /// <returns></returns>
        private float ApplyGravity(float verticalSpeed, float deltaTime)
        {
            return verticalSpeed + gravityValue * GravityMultiplier * deltaTime;
        }
    }
}
