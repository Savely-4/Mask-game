using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class Player : MonoBehaviour
{
    #region Components
    [SerializeField] PlayerMovementCFG configMove;
    InputAction mouseLook, movementAction, jumpAction, sprintAction, dashAction;
    Rigidbody rb;
    HpOnObject hpPlayer;
    #endregion
    #region Movement

    #region Move
    public float currentSpeed, coefSpeed = 1.2f;

    Vector3 move;
    Vector2 moveInput;

    private Dictionary<string, bool> cooldowns = new();
    float stamina;
    const float staminaMax = 100f;
    bool playerSprinting;
    Vector3 directionalDash;
    #endregion

    #region Dash
    //рывок
    public float distanceDash = 5f;

    float speedDash;
    bool cdDash = false;
    //public float cdDashtoTime = 3f;

    #endregion
    #region Jump

    bool onGround;
    Vector3 originRaycastJump;
    public float distanceRaycastJump = 0.75f;
    RaycastHit hitGroundOnRaycast;
    bool canDoubleJump;
    bool canJump;
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
        hpPlayer = GetComponent<HpOnObject>();
        rb = GetComponent<Rigidbody>();

        dashAction = InputSystem.actions.FindAction("Dash");
        jumpAction = InputSystem.actions.FindAction("Jump");
        mouseLook = InputSystem.actions.FindAction("Look");
        movementAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentSpeed = configMove.Speed;
        stamina = staminaMax;

        hpPlayer.hp = 100f;
        hpPlayer.maxHp = 100f;
        hpPlayer.regenRate = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Dash();
        if (jumpAction.WasPressedThisFrame())
            canJump = true;
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
        if (movementAction.IsPressed() && !cdDash)
        {
            moveInput = movementAction.ReadValue<Vector2>();
            move = transform.forward * moveInput.y + transform.right * moveInput.x;

            //Debug.Log(currentSpeed);    
            rb.MovePosition(rb.position + move * currentSpeed * Time.fixedDeltaTime);
        }
    }
    void Dash()
    {
        if (IsOnCooldown("Dash")) return;

        if (dashAction.IsPressed() && !cdDash)
        {
            directionalDash = transform.position + transform.forward * distanceDash;
            rb.MovePosition(directionalDash);
            cdDash = true;
        }
        else if (cdDash)
        {
            StartCooldown("Dash", 4f);
            cdDash = false;
        }
    }
    void Sprint()
    {
        if (IsOnCooldown("Sprint")) return;
        playerSprinting = sprintAction.IsPressed();


        if (playerSprinting && stamina > 0)
        {
            currentSpeed += coefSpeed * Time.deltaTime;
            stamina -= 20f * Time.deltaTime;

            if (stamina <= 0)
            {
                stamina = 0;
                StartCooldown("Sprint", 3f);
            }
        }
        else if (!playerSprinting)
        {
            if (stamina < staminaMax)
            {
                stamina += 10f * Time.deltaTime;
                currentSpeed -= (coefSpeed + 3) * Time.deltaTime;
            }
            if (stamina <= 0)
            {
                StartCooldown("Sprint", 1f);
            }
        }
        currentSpeed = Mathf.Clamp(currentSpeed, configMove.Speed, configMove.SprintSpeed);
        Debug.Log(stamina);
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
        onGround = Physics.Raycast(originRaycastJump, Vector3.down, out hitGroundOnRaycast, distanceRaycastJump);
        if (onGround) canDoubleJump = true;

        if (!canJump) return;

        if (onGround) rb.AddForce(0, configMove.JumpDistance, 0, ForceMode.Impulse);

        else if (canDoubleJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(0, configMove.JumpDistance, 0, ForceMode.Impulse);
            canDoubleJump = false;
        }
        canJump = false;
    }

    #region cdUnirsality
    //и для прыжка тоже кд мб
    bool IsOnCooldown(string action)
    {
        return cooldowns.ContainsKey(action) && cooldowns[action];
    }
    async void StartCooldown(string action, float duration)
    {
        if (IsOnCooldown(action)) return;

        cooldowns[action] = true;
        await Task.Delay(TimeSpan.FromSeconds(duration));
        cooldowns[action] = false;
    }
    #endregion
}
