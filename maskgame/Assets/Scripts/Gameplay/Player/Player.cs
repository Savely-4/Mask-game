using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.Collections;

public class Player : MonoBehaviour
{
    #region Components
    [SerializeField] PlayerMovementCFG configMove;
    public static Player player;
    public InputAction mouseLook, movementAction, jumpAction, sprintAction, dashAction,interact,mousePosAction;
    Rigidbody rb;
    HpOnObject hpPlayer;
    Animator playerAnimator;
    #endregion
    #region Movement

    #region Move
    public float currentSpeed, coefSpeed = 4f;

    Vector3 move;
    Vector2 moveInput;

    private Dictionary<string, bool> cooldowns = new();
    float stamina;
    const float staminaMax = 100f;
    float staminaRegenDelayTimer = 0f;
    bool playerCantSprint;
    Vector3 directionalDash;
    #endregion
    #region Dash
    public float distanceDash = 5f;
    float timeToCdDash = 4f;
    public float speedDash = 10f;
    bool cdDash = false;

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
    #region Animation
    bool nonAllEneract = false;

    #endregion
    #region BattleSystem
    [SerializeField] private Weapon currentWeapon;
    public Transform hands;
    public bool SwordInHands = false;
    #endregion


    private void Awake()
    {
        player = this;
        playerAnimator = GetComponent<Animator>();
        hpPlayer = GetComponent<HpOnObject>();
        rb = GetComponent<Rigidbody>();
        
        interact = InputSystem.actions.FindAction("Interact");
        dashAction = InputSystem.actions.FindAction("Dash");
        jumpAction = InputSystem.actions.FindAction("Jump");
        mouseLook = InputSystem.actions.FindAction("Look");
        movementAction = InputSystem.actions.FindAction("Move");
        mousePosAction = InputSystem.actions.FindAction("MousePositions");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        currentSpeed = configMove.Speed;
        stamina = staminaMax;
        cameraHolder = Camera.main.transform;
        hpPlayer.hp = 100f;
        hpPlayer.maxHp = 100f;
        hpPlayer.regenRate = 2f;
    }

    void Update()
    {
        if(interact.IsPressed() && SwordInHands)
        {
            currentWeapon = FindFirstObjectByType<Weapon>();
            currentWeapon?.Attack();
        }
        if(Mouse.current.leftButton.IsPressed()) currentWeapon?.Attack();

        Sprint();
        Dash();
        if (jumpAction.WasPressedThisFrame() && !nonAllEneract)
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
        if (movementAction.IsPressed() && !cdDash && !nonAllEneract)
        {
            moveInput = movementAction.ReadValue<Vector2>();
            move = transform.forward * moveInput.y + transform.right * moveInput.x;
   
            rb.MovePosition(rb.position + move * currentSpeed * Time.fixedDeltaTime);
        }
    }
    void Dash()
    {
        if (IsOnCooldown("Dash") || nonAllEneract) return;

        if (dashAction.IsPressed() && !cdDash && movementAction.IsPressed())
        {
            directionalDash = transform.position + new Vector3(moveInput.x,moveInput.y,0) * distanceDash;
            directionalDash.y = transform.position.y;
            Vector3 vector = Vector3.MoveTowards(transform.position, directionalDash, speedDash * Time.deltaTime);
            rb.MovePosition(vector);
            cdDash = true;
        }
        else if(!cdDash && dashAction.IsPressed())
        {
            directionalDash = transform.position + transform.forward * distanceDash;
            directionalDash.y = transform.position.y;
            Vector3 vector = Vector3.MoveTowards(transform.position, directionalDash, speedDash * Time.deltaTime);
            rb.MovePosition(vector);
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
        if (nonAllEneract) return;

        if(stamina <= 0 && !IsOnCooldown("Sprint") && !playerCantSprint)
        {
            stamina = 0;
            playerCantSprint = true;
            StartCooldown("Spint", 5f);
        }
        if (playerCantSprint && stamina >= staminaMax * 0.3f) playerCantSprint = false;

        if(sprintAction.IsPressed() && movementAction.IsPressed() && !IsOnCooldown("Sprint") && !playerCantSprint && stamina > 0)
        {
            stamina -= 20f * Time.deltaTime;

            if (stamina < 0) stamina = 0;

            if (currentSpeed < configMove.SprintSpeed) currentSpeed += coefSpeed * Time.deltaTime;
            staminaRegenDelayTimer = 1.5f;
        }
        else
        {
            if(staminaRegenDelayTimer > 0f) staminaRegenDelayTimer -= Time.deltaTime;
            else if (stamina < staminaMax) stamina += 10f * Time.deltaTime;

            if(currentSpeed > configMove.Speed) currentSpeed -= coefSpeed * Time.deltaTime;

        }
        currentSpeed = Mathf.Clamp(currentSpeed, configMove.Speed, configMove.SprintSpeed);
        Mathf.Clamp(stamina, 0f, staminaMax);
    }
    void LookAround(float mouseSens = 0.1f)
    {
        if(nonAllEneract) return;
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
    IEnumerator NonEnteract(int layer, float secondsToWait = 10)
    {
        nonAllEneract = true;
        yield return new WaitForSeconds(secondsToWait);
        nonAllEneract = false;
    }

    void TakeItem()
    {
        if(interact.IsPressed())
        {
            Vector2 mousePosition = mousePosAction.ReadValue<Vector2>();
            if(Physics.Raycast(mousePosition,Camera.main.transform.forward,out RaycastHit hitItem ,20) && hitItem.collider.tag == "Item")
            {
                Destroy(hitItem.collider.gameObject);
            }
        }
    }

    #region cdUnirsality
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