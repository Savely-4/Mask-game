using UnityEngine;

//TODO: Add state machine on top
public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;

    private Vector2 movementInput;
    private float verticalVelocityValue = 0;

    /// <summary>
    /// Time for which movement input was remaining unchanged
    /// </summary>
    private float timeSinceInputChanged;

    [Header("Movement parameters")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float acceleration = 5f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private float gravityValue = -9.81f;

    [field: Header("Multipliers")]
    // [field: SerializeField] public float SpeedMultiplier { get; set; } = 1;
    [field: SerializeField] public float GravityMultiplier { get; set; } = 1;

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

    public void SetMovementInput(Vector2 value)
    {
        movementInput = value;
        timeSinceInputChanged = 0;
    }

    public void Jump()
    {
        verticalVelocityValue = Mathf.Sqrt(jumpHeight * -2 * gravityValue * GravityMultiplier);
    }

    private Vector3 GetVelocity()
    {
        var inputVelocity = GetInputVelocity();
        var lerpValue = GetLerpValueFromTime(timeSinceInputChanged, speed / acceleration);

        var velocityHorizontal = Vector3.Lerp(_characterController.velocity, inputVelocity, lerpValue);
        velocityHorizontal.y = 0;

        var velocityVertical = verticalVelocityValue * Vector3.up;

        return velocityHorizontal + velocityVertical;
    }


    /// <summary>
    /// Get desired horizontal velocity based on input
    /// </summary>
    private Vector3 GetInputVelocity()
    {
        var inputWorld = (movementInput.x * transform.forward + movementInput.y * transform.right).normalized;

        //calculate speed based on current speed and acceleration
        var speed = this.speed;

        return speed * inputWorld;
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
