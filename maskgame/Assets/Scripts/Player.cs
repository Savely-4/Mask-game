using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.STP;

public class Player : MonoBehaviour
{
    #region Components
    [SerializeField] PlayerMovementCFG configMove;
    InputAction mouseLook, movementAction, jumpActions , sprintAction;
    Rigidbody rb;
    #endregion
    #region Movement

    #region Move
    public float currentSpeed,coefSpeed = 1.2f;

    Vector3 move;
    Vector2 moveInput;

    float stamina;
    const float staminaMax = 100f;
    #endregion

    #region Jump
    bool onGround;
    Vector3 originRaycastJump;
    public float distanceRaycastJump = 0.75f;
    RaycastHit hitGroundOnRaycast;
    int jumpCount;
    const int maxJumps = 2;
    #endregion

    #endregion


    #region CameraLook
    public Transform cameraHolder;
    private float xRotation = 0f;
    Vector2 look;
    #endregion

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        jumpActions = InputSystem.actions.FindAction("Jump");
        mouseLook = InputSystem.actions.FindAction("Look");
        movementAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentSpeed = configMove.Speed;
        stamina = staminaMax;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
    }
    private void LateUpdate()
    {
        LookAround();
    }
    private void FixedUpdate()
    {
        Movement();
        Jump();
    }
    void Movement()
    {
        if (movementAction.IsPressed())
        {

            moveInput = movementAction.ReadValue<Vector2>();
            move = transform.forward * moveInput.y + transform.right * moveInput.x;

            //Debug.Log(currentSpeed);    
            rb.MovePosition(rb.position + move * currentSpeed * Time.fixedDeltaTime);
        }
    }
    void Sprint()
    {
        if (sprintAction.IsPressed() && stamina > 0)
        {
            currentSpeed += 15f * Time.deltaTime;
            stamina -= 20f * Time.deltaTime;

        }
        else if(!sprintAction.IsPressed())
        {
            currentSpeed -= coefSpeed * Time.deltaTime;
            if(stamina < staminaMax)
            {
                stamina -= 20f * Time.deltaTime;
            }
            
        }
        else if(stamina <= 0)
        {

        }

        Debug.Log(stamina);
        currentSpeed = Mathf.Clamp(currentSpeed, configMove.Speed, configMove.SprintSpeed);
    }
    void LookAround(float mouseSens = 0.1f)
    {
        look = mouseLook.ReadValue<Vector2>() * mouseSens;
        xRotation -= look.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * look.x);
        
    }
    void Jump()
    {
        originRaycastJump = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * 0.5f), transform.position.z);
        if (Physics.Raycast(originRaycastJump, transform.TransformDirection(Vector3.down), out hitGroundOnRaycast, distanceRaycastJump))
        {
            //Debug.DrawRay(originRaycastJump, transform.TransformDirection(Vector3.down) * distanceRaycastJump, Color.red);
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        if (jumpActions.IsPressed() && onGround)
        {
            onGround = false;
            rb.AddForce(0, configMove.JumpDistance, 0, ForceMode.Impulse);
        }
    }

}
