using UnityEngine;

//TODO: Add state machine on top
public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;

    private Vector2 movementInput;
    private bool sprintInput = false;

    private float verticalVelocityValue = 0;

    /// <summary>
    /// Time for which movement input was remaining unchanged
    /// </summary>
    private float timeSinceInputChanged;

    [Header("Movement parameters")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float acceleration = 30f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float jumpStopThreshold = 0.2f;

    //TODO: Add ability to change multipliers
    [field: Header("Multipliers")]
    [field: SerializeField] public float SpeedMultiplier { get; private set; } = 1;
    [field: SerializeField] public float GravityMultiplier { get; private set; } = 1;

    public Vector3 Speed => _characterController.velocity;
    public bool IsGrounded => _characterController.isGrounded;


    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        timeSinceInputChanged += Time.fixedDeltaTime;

        verticalVelocityValue = ApplyGravity(verticalVelocityValue, Time.fixedDeltaTime);

        var velocity = GetVelocity();

        _characterController.Move(velocity * Time.fixedDeltaTime);

        if (IsGrounded)
            verticalVelocityValue = _characterController.velocity.y;
    }


    public void SetMovementInput(Vector2 value)
    {
        movementInput = value;
        timeSinceInputChanged = 0;
    }

    public void ToggleSprint(bool value)
    {
        if (sprintInput == value)
            return;

        sprintInput = value;
        timeSinceInputChanged = 0;
    }

    public void Jump()
    {
        verticalVelocityValue = Mathf.Sqrt(jumpHeight * -2 * gravityValue * GravityMultiplier);
        timeSinceInputChanged = 0;
    }

    /// <summary>
    /// When called, if jumping - stop ascending and start falling prematurely
    /// </summary>
    public void StopJump()
    {
        //if called - check if not grounded and flying up
        //Reduce vertical speed to small positive value
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
        var inputWorld = (movementInput.x * transform.forward + movementInput.y * transform.right).normalized;

        return speed * inputWorld;
    }

    /// <summary>
    /// Get desired horizontal speed based on input
    /// </summary>
    private float GetHorizontalSpeed()
    {
        if (sprintInput)
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
